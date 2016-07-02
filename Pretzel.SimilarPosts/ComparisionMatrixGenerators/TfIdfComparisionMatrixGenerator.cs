using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pretzel.Logic.Templating.Context;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.ComparisionMatrixGenerators
{
    public class TfIdfComparisionMatrixGenerator : IComparisionMatrixGenerator
    {
        protected readonly IComparision[] Comparisions;

        public TfIdfComparisionMatrixGenerator(IComparision[] comparisions)
        {
            Comparisions = comparisions;
        }

        public Dictionary<string, Dictionary<string, double>> Generate(IDictionary<string, string[]> words, IList<Page> documents)
        {
            // File name - TF
            var tf = words.ToDictionary(source => source.Key, source => CalcTf(source.Value));

            // Word - IDF
            var idf = CalcIdf(words);

            // File name - word - TFIDF (TextVector)
            var tfidf = CalcTfIdf(tf, idf);

            // Compare documents to each other
            // File name - File name - Relevance
            string[] filesList = tfidf.Select(file => file.Key).ToArray();
            return filesList
                    .ToDictionary(file1 => file1, file1 => filesList.Where(file => file != file1)
                    .ToDictionary(source => source, file2 =>
                    {
                        double result = 0;
                        foreach (IComparision comparision in Comparisions)
                        {
                            result += comparision.Compare(documents, file1, file2, tfidf[file1], tfidf[file2]);
                        }
                        return result;
                    }));
        }

        private static TextVector CalcTf(string[] words)
        {
            var result = new TextVector();
            double len = words.Length;
            foreach (var word in words.Distinct())
            {
                result.Add(word, words.Count(x => x == word) / len);
            }
            return result;
        }

        private static TextVector CalcIdf(IDictionary<string, string[]> files)
        {
            var result = new TextVector();

            double len = files.Count();
            foreach (var word in files.SelectMany(file => file.Value).Distinct())
            {
                int count = files.Count(file => file.Value.Contains(word));
                double idf = (double)Math.Log10(len / count);
                result.Add(word, idf);
            }

            return result;
        }

        private static Dictionary<string, TextVector> CalcTfIdf(IDictionary<string, TextVector> tf, TextVector idf)
        {
            var result = new Dictionary<string, TextVector>();

            foreach (var file in tf)
            {
                var words = file.Value
                    .ToDictionary(source => source.Key, word =>
                    {
                        double tfValue = word.Value;
                        double idfValue = 0;
                        idf.TryGetValue(word.Key, out idfValue);
                        return tfValue * idfValue;
                    });

                result.Add(file.Key, new TextVector(words));
            }

            return result;
        }
    }
}
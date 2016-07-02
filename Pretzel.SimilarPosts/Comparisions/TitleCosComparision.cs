using System;
using System.Collections.Generic;
using System.Linq;
using Pretzel.Logic.Templating.Context;
using Pretzel.SimilarPosts.Api;
using Pretzel.SimilarPosts.SourceCleanups;
using Pretzel.SimilarPosts.WordCleanups;

namespace Pretzel.SimilarPosts.Comparisions
{
    public class TitleCosComparision : IComparision
    {
        protected readonly double Weight;
        protected readonly ISourceCleanup[] SourceCleanups;
        protected readonly IWordCleanup[] WordCleanups;

        public TitleCosComparision(double weight)
            : this(weight,
                   new ISourceCleanup[] { new CleanupHtml(),
                                          new ReservedDictionary(new [] { ".net", "asp.net", "vs.net"}),
                                          new RemoveNotLetters(),
                                          new RemoveNumbers() },
                   new IWordCleanup[] { new StemCleanup() })
        {
        }

        public TitleCosComparision(double weight, ISourceCleanup[] sourceCleanups, IWordCleanup[] wordCleanups)
        {
            Weight = weight;
            SourceCleanups = sourceCleanups;
            WordCleanups = wordCleanups;
        }

        public double Compare(IList<Page> allDocuments, string name1, string name2, TextVector document1,
            TextVector document2)
        {
            var page1 = allDocuments.FirstOrDefault(page => page.File == name1);
            var page2 = allDocuments.FirstOrDefault(page => page.File == name2);

            if ((page1 == null) || (page2 == null))
                return 0;

            string title1 = page1.Title;
            string title2 = page2.Title;
            if (SourceCleanups != null)
            {
                foreach (var sourceCleanup in SourceCleanups)
                {
                    title1 = sourceCleanup.Cleanup(title1);
                    title2 = sourceCleanup.Cleanup(title2);
                }
            }

            string[] words1 = ExtractWords(title1);
            string[] words2 = ExtractWords(title2);

            var vector1 = words1.Distinct().ToTextVector(word => word, word => (double)words1.Count(x => x == word));
            var vector2 = words2.Distinct().ToTextVector(word => word, word => (double)words2.Count(x => x == word));

            return Weight * Compare(vector1, vector2);
        }

        private double Compare(TextVector vector1, TextVector vector2)
        {
            var intersect = vector1.Keys.Intersect(vector2.Keys).ToArray();
            var all = vector1.Keys.Concat(vector2.Keys).Distinct().ToArray();

            double sum = 0;
            if (intersect.Any())
            {
                sum = intersect.Select(word => vector1[word] * vector2[word]).Sum();
            }

            double len = 0;
            if (all.Any())
            {
                len = Math.Sqrt(all.Select(word => Pow(vector1, word)).Sum()) *
                      Math.Sqrt(all.Select(word => Pow(vector2, word)).Sum());
            }

            return Weight * ((sum > 0) && (len > 0) ? sum / len : 0);
        }

        private string[] ExtractWords(string source)
        {
            return (source ?? String.Empty)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(word => word.Length > 2)
                .Select(word =>
                {
                    string result = word;
                    if (WordCleanups != null)
                    {
                        foreach (var wordCleanup in WordCleanups)
                        {
                            result = wordCleanup.Cleanup(result);
                        }
                    }
                    return result;
                })
                .ToArray();
        }

        private static double Pow(TextVector vector, string word)
        {
            double x = 0;
            vector.TryGetValue(word, out x);
            return x * x;
        }
    }
}
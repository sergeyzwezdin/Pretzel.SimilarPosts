using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pretzel.Logic.Templating.Context;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.Comparisions
{
    public class TextVectorCosComparision : IComparision
    {
        protected readonly double Weight;

        public TextVectorCosComparision(double weight)
        {
            Weight = weight;
        }

        public double Compare(IList<Page> allDocuments, string name1, string name2, TextVector document1, TextVector document2)
        {
            var intersect = document1.Keys.Intersect(document2.Keys).ToArray();
            var all = document1.Keys.Concat(document2.Keys).Distinct().ToArray();

            double sum = 0;
            if (intersect.Any())
            {
                sum = intersect.Select(word => document1[word] * document2[word]).Sum();
            }

            double len = 0;
            if (all.Any())
            {
                len = Math.Sqrt(all.Select(word => Pow(document1, word)).Sum()) *
                      Math.Sqrt(all.Select(word => Pow(document2, word)).Sum());
            }

            return Weight * ((sum > 0) && (len > 0) ? sum / len : 0);
        }

        private static double Pow(TextVector document, string word)
        {
            double x = 0;
            document.TryGetValue(word, out x);
            return x * x;
        }
    }
}

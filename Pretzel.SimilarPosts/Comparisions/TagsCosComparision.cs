using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pretzel.Logic.Templating.Context;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.Comparisions
{
    public class TagsCosComparision : IComparision
    {
        protected readonly double Weight;

        public TagsCosComparision(double weight)
        {
            Weight = weight;
        }

        public double Compare(IList<Page> allDocuments, string name1, string name2, TextVector document1, TextVector document2)
        {
            var page1 = allDocuments.FirstOrDefault(page => page.File == name1);
            var page2 = allDocuments.FirstOrDefault(page => page.File == name2);

            if ((page1 == null) || (page2 == null))
                return 0;

            var intersect = page1.Tags.Intersect(page2.Tags).ToArray();
            var all = page1.Tags.Concat(page2.Tags).Distinct().ToArray();

            double sum = 0;
            if (intersect.Any())
            {
                sum = intersect.Select(category => (page1.Tags.Contains(category) ? 1 : 0) * (page2.Tags.Contains(category) ? 1 : 0)).Sum();
            }

            double len = 0;
            if (all.Any())
            {
                len = Math.Sqrt(all.Select(category => (page1.Tags.Contains(category) ? 1 : 0)).Sum()) *
                      Math.Sqrt(all.Select(category => (page2.Tags.Contains(category) ? 1 : 0)).Sum());
            }

            return Weight * ((sum > 0) && (len > 0) ? sum / len : 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pretzel.Logic.Templating.Context;

namespace Pretzel.SimilarPosts.Api
{
    public interface IComparisionMatrixGenerator
    {
        Dictionary<string, Dictionary<string, double>> Generate(IDictionary<string, string[]> words, IList<Page> documents);
    }
}

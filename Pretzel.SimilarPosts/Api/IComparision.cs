using System.Collections.Generic;
using Pretzel.Logic.Templating.Context;

namespace Pretzel.SimilarPosts.Api
{
    public interface IComparision
    {
        double Compare(IList<Page> allDocuments, string name1, string name2, TextVector document1, TextVector document2);
    }
}
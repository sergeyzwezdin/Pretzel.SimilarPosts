using System.Collections.Generic;

namespace Pretzel.SimilarPosts.Api
{
    public interface IContentParser
    {
        IDictionary<string, string> Parse(string[] files, ISourceCleanup[] sourceCleanups);
    }
}
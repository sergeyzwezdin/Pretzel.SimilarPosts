using System.Text.RegularExpressions;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.SourceCleanups
{
    public class RemoveNumbers : ISourceCleanup
    {
        private static readonly Regex RemoveNumbersRegex = new Regex(@"[0-9]+",
            RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Compiled);

        public string Cleanup(string source)
        {
            var result = source;
            result = RemoveNumbersRegex.Replace(result, " ");
            return result;
        }
    }
}
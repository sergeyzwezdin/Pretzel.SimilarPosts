using System.Text.RegularExpressions;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.SourceCleanups
{
    public class RemoveNotLetters : ISourceCleanup
    {
        private static readonly Regex RemoveNotLettersRegex = new Regex(@"\W+",
            RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Compiled);

        public string Cleanup(string source)
        {
            var result = source;
            result = RemoveNotLettersRegex.Replace(result, " ");
            return result;
        }
    }
}
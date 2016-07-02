using System.Text.RegularExpressions;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.SourceCleanups
{
    public class CleanupHtml : ISourceCleanup
    {
        private static readonly Regex CleanupHtmlRegex = new Regex(@"<[^>]*>",
            RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Compiled);

        public string Cleanup(string source)
        {
            return CleanupHtmlRegex.Replace(source ?? string.Empty, string.Empty).ToLowerInvariant();
        }
    }
}
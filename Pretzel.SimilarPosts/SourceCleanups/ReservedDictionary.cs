using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.SourceCleanups
{
    public class ReservedDictionary : ISourceCleanup
    {
        private static readonly Regex RemoveNotLettersRegex = new Regex(@"\W+",
            RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Compiled);

        protected readonly IDictionary<string, string> Dictionary;

        public ReservedDictionary(string[] reserved)
        {
            Dictionary = reserved.ToDictionary(source => source,
                                               source => RemoveNotLettersRegex.Replace(source, string.Empty));
        }

        public string Cleanup(string source)
        {
            var result = source;
            foreach (var replace in Dictionary)
            {
                result = result.Replace(replace.Key, replace.Value);
            }
            return result;
        }
    }
}
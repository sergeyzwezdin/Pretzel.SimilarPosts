using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts
{
    public class ContentParser : IContentParser
    {
        private static readonly Regex YamlHeaderRegex = new Regex(@"(?s:^---(.*?)---)", RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Compiled);

        public IDictionary<string, string> Parse(string[] files, ISourceCleanup[] sourceCleanups)
        {
            return (files ?? new string[] {})
                .ToDictionary(file => file, file =>
                {
                    string content = File.ReadAllText(file);
                    content = RemoveYamlHeader(content);

                    if (sourceCleanups != null)
                    {
                        foreach (var sourceCleanup in sourceCleanups)
                        {
                            content = sourceCleanup.Cleanup(content);
                        }
                    }

                    return content;
                });
        }

        private static string RemoveYamlHeader(string source)
        {
            MatchCollection matches = YamlHeaderRegex.Matches(source);
            if (matches.Count == 0)
                return source;

            return source.Replace(matches[0].Groups[0].Value, String.Empty)
                .TrimStart('\r', '\n')
                .TrimEnd();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Pretzel.Logic;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;
using Pretzel.SimilarPosts.Api;
using Pretzel.SimilarPosts.ComparisionMatrixGenerators;
using Pretzel.SimilarPosts.Comparisions;
using Pretzel.SimilarPosts.Helpers;
using Pretzel.SimilarPosts.SourceCleanups;
using Pretzel.SimilarPosts.WordCleanups;

namespace Pretzel.SimilarPosts
{
    public class SimilarPostsProcessingTransform : IBeforeProcessingTransform
    {
        protected readonly IConfiguration Config;
        protected readonly IContentParser ContentParser;
        protected readonly ISourceCleanup[] SourceCleanups;
        protected readonly IWordCleanup[] WordCleanups;
        protected readonly IComparisionMatrixGenerator Generator;

        [ImportingConstructor]
        public SimilarPostsProcessingTransform(IConfiguration config)
            : this(config,
                   new ContentParser(),
                   new ISourceCleanup[] { new CleanupHtml(),
                                          new ReservedDictionary(config.GetConfigReservedWords()),
                                          new RemoveNotLetters(),
                                          new RemoveNumbers() },
                   new IWordCleanup[] { new StemCleanup(config.GetConfigStemmers()) },
                   new TfIdfComparisionMatrixGenerator(new IComparision[] { new CategoriesCosComparision(config.GetConfigWeight("categories", 0.1)),
                                                                            new TagsCosComparision(config.GetConfigWeight("tags", 0.3)),
                                                                            new TitleCosComparision(config.GetConfigWeight("title", 0.1)),
                                                                            new TextVectorCosComparision(config.GetConfigWeight("text", 1)) })
                   )
        {
        }


        public SimilarPostsProcessingTransform(IConfiguration config, IContentParser contentParser, ISourceCleanup[] sourceCleanups, IWordCleanup[] wordCleanups, IComparisionMatrixGenerator generator)
        {
            Config = config;
            ContentParser = contentParser;
            SourceCleanups = sourceCleanups;
            WordCleanups = wordCleanups;
            Generator = generator;
        }

        public void Transform(SiteContext context)
        {
            // File name - file content
            var filesContent = ContentParser.Parse(context.Posts.Select(x => x.File).ToArray(), SourceCleanups);

            // File name - file words
            var words = filesContent.Select(file =>
            {
                string[] result = ExtractWords(file.Value);
                result = result.Select(word =>
                {
                    if (WordCleanups != null)
                    {
                        foreach (var wordCleanup in WordCleanups)
                        {
                            word = wordCleanup.Cleanup(word);
                        }
                    }
                    return word;
                }).ToArray();
                return new KeyValuePair<string, string[]>(file.Key, result);
            }).ToDictionary(source => source.Key, source => source.Value);

            // Compare documents to each other
            // File name - File name - Relevance
            Dictionary<string, Dictionary<string, double>> compareMatrix = Generator.Generate(words, context.Posts);

            // Modify posts collection
            int relatedCount = Config.GetConfigIntValue("related_count", 3);
            double threshold = Config.GetConfigDoubleValue("filter_threshold", 0.1);

            foreach (var post in context.Posts)
            {
                post.Bag["related"] = GetRelatedPosts(compareMatrix[post.File], context.Posts, relatedCount, threshold);
            }
        }

        static IEnumerable<Page> GetRelatedPosts(Dictionary<string, double> relatedPosts, IList<Page> pages, int relatedCount, double threshold)
        {
            var files = relatedPosts
                .OrderByDescending(post => post.Value)
                .Where(post => post.Value >= threshold)
                .Take(relatedCount)
                .Select(post => post.Key)
                .ToArray();

            return files
                .Select(file => pages.FirstOrDefault(page => page.File == file))
                .Where(page => page != null).ToArray();
        }

        static string[] ExtractWords(string source)
        {
            return (source ?? String.Empty)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(word => word.Length > 2)
                .ToArray();
        }
    }
}
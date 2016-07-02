using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iveonik.Stemmers;
using Pretzel.SimilarPosts.Api;

namespace Pretzel.SimilarPosts.WordCleanups
{
    public class StemCleanup : IWordCleanup
    {
        protected readonly IStemmer[] Stemmers;

        public StemCleanup()
            : this(null)
        {

        }

        public StemCleanup(string[] stemmers)
        {
            if (stemmers == null)
            {
                Stemmers = new IStemmer[] { };
                return;
            }

            var types = typeof(IStemmer)
                .Assembly
                .GetTypes()
                .Where(x => typeof(IStemmer).IsAssignableFrom(x))
                .ToArray();

            Stemmers = stemmers
                .Select(name => types.FirstOrDefault(type => type.Name == name + "Stemmer"))
                .Where(type => type != null)
                .Select(type => (IStemmer)Activator.CreateInstance(type))
                .ToArray();
        }

        public string Cleanup(string source)
        {
            string result = source;
            foreach (var stemmer in Stemmers)
            {
                result = stemmer.Stem(result);
            }
            return result;
        }
    }
}

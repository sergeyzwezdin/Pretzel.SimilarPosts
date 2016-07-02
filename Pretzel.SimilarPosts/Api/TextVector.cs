using System;
using System.Collections.Generic;

namespace Pretzel.SimilarPosts.Api
{
    public class TextVector : Dictionary<string, double>
    {
        public TextVector()
        {
        }

        public TextVector(Dictionary<string, double> source)
            : base(source)
        {
        }
    }

    public static class TextVectorExtensions
    {
        public static TextVector ToTextVector<T>(this IEnumerable<T> source, Func<T, string> keySelector, Func<T, double> elementSelector)
        {
            if (source == null)
                return null;

            TextVector result = new TextVector();

            foreach (var item in source)
            {
                result.Add(keySelector(item), elementSelector(item));
            }

            return result;
        }
    }
}
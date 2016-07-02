using System.Collections.Generic;
using System.Globalization;
using Pretzel.Logic;

namespace Pretzel.SimilarPosts.Helpers
{
    public static class ConfigExtensions
    {
        public static int GetConfigIntValue(this IConfiguration config, string key, int defaultValue)
        {
            if (config.ContainsKey("similar_posts") == false)
                return defaultValue;

            var pluginConfig = config["similar_posts"] as Dictionary<string, object>;
            if (pluginConfig == null)
                return defaultValue;

            if (pluginConfig.ContainsKey(key) == false)
                return defaultValue;

            var keySection = pluginConfig[key] as string;
            if (keySection == null)
                return defaultValue;

            int result = defaultValue;
            int.TryParse(keySection, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static double GetConfigDoubleValue(this IConfiguration config, string key, double defaultValue)
        {
            if (config.ContainsKey("similar_posts") == false)
                return defaultValue;

            var pluginConfig = config["similar_posts"] as Dictionary<string, object>;
            if (pluginConfig == null)
                return defaultValue;

            if (pluginConfig.ContainsKey(key) == false)
                return defaultValue;

            var keySection = pluginConfig[key] as string;
            if (keySection == null)
                return defaultValue;

            double result = defaultValue;
            double.TryParse(keySection, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static double GetConfigWeight(this IConfiguration config, string key, double defaultValue)
        {
            if (config.ContainsKey("similar_posts") == false)
                return defaultValue;

            var pluginConfig = config["similar_posts"] as Dictionary<string, object>;
            if (pluginConfig == null)
                return defaultValue;

            if (pluginConfig.ContainsKey("weight") == false)
                return defaultValue;

            var weightSection = pluginConfig["weight"] as Dictionary<string, object>;
            if (weightSection == null)
                return defaultValue;

            if (weightSection.ContainsKey(key) == false)
                return defaultValue;

            var keySection = weightSection[key] as string;
            if (keySection == null)
                return defaultValue;

            double result = defaultValue;
            double.TryParse(keySection, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static string[] GetConfigStemmers(this IConfiguration config)
        {
            var defaultValue = new string[] { };

            if (config.ContainsKey("similar_posts") == false)
                return defaultValue;

            var pluginConfig = config["similar_posts"] as Dictionary<string, object>;
            if (pluginConfig == null)
                return defaultValue;

            if (pluginConfig.ContainsKey("stemmers") == false)
                return defaultValue;

            var reservedSection = pluginConfig["stemmers"] as List<string>;
            if (reservedSection == null)
                return defaultValue;

            return reservedSection.ToArray();
        }

        public static string[] GetConfigReservedWords(this IConfiguration config)
        {
            var defaultValue = new string[] { };

            if (config.ContainsKey("similar_posts") == false)
                return defaultValue;

            var pluginConfig = config["similar_posts"] as Dictionary<string, object>;
            if (pluginConfig == null)
                return defaultValue;

            if (pluginConfig.ContainsKey("reserved") == false)
                return defaultValue;

            var reservedSection = pluginConfig["reserved"] as List<string>;
            if (reservedSection == null)
                return defaultValue;

            return reservedSection.ToArray();
        }
    }
}

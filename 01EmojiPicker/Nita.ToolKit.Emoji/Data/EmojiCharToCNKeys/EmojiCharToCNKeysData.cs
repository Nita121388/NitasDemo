using Newtonsoft.Json;
using Nita.ToolKit.Emoji.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.Emoji.Data.EmojiCharToCNKeys
{
    /// <summary>
    /// Emoji Chars TO 中文key
    /// </summary>
    public class EmojiCharToCNKeysData
    {
        private const string resourcePath = "Nita.ToolKit.Emoji.Data.EmojiCharToCNKeys.";
        private const string resourceName = "EmojiCharToCNKeys.json";
        private static List<EmojiCharToCNKeys> EmojiCharCNKeys = new List<EmojiCharToCNKeys>();
        private static Dictionary<string, List<string>> EmojiCharCNKeysCache = new Dictionary<string, List<string>>();

        static EmojiCharToCNKeysData()
        {
            EmojiCharCNKeys = ReadCNKeys(resourceName);
        }

        public static List<string> GetValue(string emojiChar)
        {
            if (EmojiCharCNKeysCache.TryGetValue(emojiChar, out var cachedValue))
            {
                return cachedValue;
            }

            var matchingCNKeys = EmojiCharCNKeys
                .Where(e => e.Char == emojiChar)
                .SelectMany(e => e.CNKeys)
                .Distinct();

            var result = matchingCNKeys.Any() ? matchingCNKeys.ToList() : new List<string>();

            if (result.Count > 0)
            {
                EmojiCharCNKeysCache.Add(emojiChar, result);// [emojiChar] = result;
            }

            return result;
        }

        public static List<EmojiCharToCNKeys> ReadCNKeys(string resourceName)
        {
            using (Stream stream = AssemblyResourceHelper.Get(resourcePath, resourceName))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                Dictionary<string, List<string>> dict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                List<EmojiCharToCNKeys> list = new List<EmojiCharToCNKeys>();

                foreach (var item in dict)
                {
                    EmojiCharToCNKeys cNKeyEmojiChars = new EmojiCharToCNKeys
                    {
                        Char = item.Key.Trim(),
                        CNKeys = item.Value
                    };
                    list.Add(cNKeyEmojiChars);
                }

                return list;
            }
        }
    }

    public class EmojiCharToCNKeys
    {
        /// <summary>
        /// 👙
        /// </summary>
        public string Char { get; set; }
        /// <summary>
        ///  ["服裝","服装","泳裝","泳装","比基尼"],
        /// </summary>
        public List<string> CNKeys { get; set; }
    }
}

using Newtonsoft.Json;
using Nita.ToolKit.Emoji.Data.CNEmojiData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Nita.ToolKit.Emoji.Data.CNToEmojiChars
{
    /* JSON Demo:
     * {
        "酒杯": ["🍸","🍷"],
        "信箱":["📫","📪"]
        }
    */
    /// <summary>
    /// 中文key,Emoji Chars数据
    /// </summary>
    public class CNToEmojiCharsData
    {
        private const string resourceFolder = "Nita.ToolKit.Emoji.Data.CNToEmojiChars.";
        private const string resourceName = "CNToEmojiChars.json";
        private static List<CNToEmojiChars> EmojiCNKeys = new List<CNToEmojiChars>();
        private static Dictionary<string, List<string>> EmojiCNKeysCache = new Dictionary<string, List<string>>();

        static CNToEmojiCharsData()
        {
            EmojiCNKeys = ReadJsonFile(resourceName);
        }

        public static List<string> GetValue(string key)
        {
            key = key.Trim();
            // 检查缓存
            if (EmojiCNKeysCache.TryGetValue(key, out var cachedValue))
            {
                return cachedValue;
            }
            var emojiChars = new List<string>();

            emojiChars = EmojiCNKeys.Where(e => e.CNKey.Contains(key))
                               .SelectMany(cNKeyEmojiChars => cNKeyEmojiChars.Chars)
                               .Distinct()
                               .ToList();
            if (emojiChars.Count <= 0)
            {
                var keyChars = key.ToCharArray();
                foreach (var keychar in keyChars)
                {
                    var tempResult = EmojiCNKeys.Where(e => e.CNKey.Contains(key))
                               .SelectMany(cNKeyEmojiChars => cNKeyEmojiChars.Chars)
                               .Distinct()
                               .ToList();
                    emojiChars.AddRange(tempResult);
                }
            }
            if (!EmojiCNKeysCache.ContainsKey(key))
            {
                EmojiCNKeysCache.Add(key, emojiChars);
            }
            return emojiChars;
        }

        public static IEnumerable<string> GetMatchingEmojiStream(string key)
        {
            // 检查缓存
            if (EmojiCNKeysCache.TryGetValue(key, out var cachedValue))
            {
                foreach (var emoji in cachedValue)
                {
                    yield return emoji;
                }
                yield break;
            }

            var emojis = EmojiCNKeys;

            var EmojiCNKeyList = emojis.Where(e => e.CNKey.Contains(key)).ToList();

            if (EmojiCNKeyList.Count > 0)
            {
                foreach (var emoji in EmojiCNKeyList.First().Chars.Distinct())
                {
                    yield return emoji;
                }
            }
            else
            {
                var keyChars = key.ToCharArray();
                foreach (var keychar in keyChars)
                {
                    var charEmojiCNKeyList = emojis.Where(e => e.CNKey.Contains(keychar)).ToList();
                    if (charEmojiCNKeyList.Count > 0)
                    {
                        foreach (var emoji in charEmojiCNKeyList.First().Chars.Distinct())
                        {
                            yield return emoji;
                        }
                    }
                }
            }
        }

        public static List<CNToEmojiChars> ReadJsonFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = resourceFolder + resourceName;

            // 检查资源是否存在
            if (!assembly.GetManifestResourceNames().Contains(resourcePath))
            {
                throw new ArgumentException($"Resource '{resourceName}' not found in the assembly.");
            }
            var stream = assembly.GetManifestResourceStream(resourcePath);
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                Dictionary<string, List<string>> dict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                List<CNToEmojiChars> list = new List<CNToEmojiChars>();

                foreach (var item in dict)
                {
                    CNToEmojiChars cNKeyEmojiChars = new CNToEmojiChars
                    {
                        CNKey = item.Key.Trim(),
                        Chars = item.Value
                    };
                    list.Add(cNKeyEmojiChars);
                }

                return list;
            }
        }
    }


    public class CNToEmojiChars
    {
        /// <summary>
        /// 中文关键词
        /// </summary>
        public string CNKey { get; set; }
        /// <summary>
        /// emoji Chars  "🍸","🍷"
        /// </summary>
        public List<string> Chars { get; set; }
    }

}

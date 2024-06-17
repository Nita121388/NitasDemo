using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Nita.ToolKit.Emoji.Data.CNEmojiData
{
    internal class CNEmojiData
    {
        private const string resourceFolder = "Nita.ToolKit.Emoji.Data.CNEmojiData.";
        private const string resourceName = "CNEmojiData.json";
        private static List<CNEmoji> cNEmojis = new List<CNEmoji>();

        static CNEmojiData()
        {
            cNEmojis = ReadJson<List<CNEmoji>>(resourceName);
        }

        static List<CNEmoji> GetAll()
        {
            return cNEmojis;
        }

        public static string GetValue(string codes, string key)
        {
            var emojis = cNEmojis;
            var emoji = emojis.Find(e => e.Char == codes);
            if (emoji != null)
            {
                switch (key)
                {
                    case "group_i18n":
                        return emoji.Group_i18n["zh_CN"];
                    case "subgroup_i18n":
                        return emoji.Subgroup_i18n["zh_CN"];
                    case "name_i18n":
                        return emoji.Name_i18n["zh_CN"];
                    default:
                        return null;
                }
            }
            return null;
        }

        public static void AddOrUpdate(CNEmoji emoji)
        {
            var emojis = GetAll();
            var index = emojis.FindIndex(e => e.Codes == emoji.Codes);
            if (index != -1)
            {
                emojis[index] = emoji;
            }
            else
            {
                emojis.Add(emoji);
            }
            SaveToFile(emojis);
        }

        public static void Delete(string codes)
        {
            var emojis = GetAll();
            var index = emojis.FindIndex(e => e.Codes == codes);
            if (index != -1)
            {
                emojis.RemoveAt(index);
                SaveToFile(emojis);
            }
        }

        private static void SaveToFile(List<CNEmoji> emojis)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var res = resourceFolder + resourceName;
            using (Stream stream = assembly.GetManifestResourceStream(res))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string json = JsonConvert.SerializeObject(emojis, Formatting.Indented);
                    writer.Write(json);
                }
            }
        }

        private static T ReadJson<T>(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var res = resourceFolder + resourceName;
            using (Stream stream = assembly.GetManifestResourceStream(res))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }

    /// <summary>
    //  "codes": "1F600",
    //  "char": "😀",
    //  "name": "grinning face",
    //  "group": "Smileys & Emotion",
    //  "subgroup": "face-smiling",
    //  "group_i18n": {
    //    "en": "Smileys & Emotion",
    //    "zh_CN": "表情与情感"
    //  },
    //  "subgroup_i18n": {
    //  "en": "face-smiling",
    //    "zh_CN": "脸-微笑"
    //  },
    //  "name_i18n": {
    //  "en": "grinning face",
    //    "zh_CN": "笑脸"
    //  }
    //}
    /// </summary>
    public class CNEmoji
    {
        public string Codes { get; set; }
        public string Char { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Subgroup { get; set; }
        public Dictionary<string, string> Group_i18n { get; set; }
        public Dictionary<string, string> Subgroup_i18n { get; set; }
        public Dictionary<string, string> Name_i18n { get; set; }
    }
}

using Nita.ToolKit.Emoji.Data.CNToEmojiChars;
using Nita.ToolKit.Emoji.Data.CNEmojiData;
using Nita.ToolKit.Emoji.Font;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Nita.ToolKit.Emoji.Data.EmojiCharToCNKeys;
using System.Collections;

namespace Nita.ToolKit.Emoji.Data
{
    //CNToEmojiCharsData
    public static class EmojiData
    {
        #region Fileds

        private static bool? m_use_custom_flags = null;

        private static string _fontName;

        private static string m_match_one_string;

        // FIXME: this could be read directly from emoji-test.txt.gz
        private static List<string> SkinToneComponents = new List<string>
        {
            "🏻", // light skin tone
            "🏼", // medium-light skin tone
            "🏽", // medium skin tone
            "🏾", // medium-dark skin tone
            "🏿", // dark skin tone
        };

        private static List<string> HairStyleComponents = new List<string>
        {
            "🦰", // red hair
            "🦱", // curly hair
            "🦳", // white hair
            "🦲", // bald
        };
        #endregion

        #region Constructors
        // FIXME: should we lazy load this? If the user calls Load() later, then
        // this first Load() call will have been for nothing.
        static EmojiData() => Load();
        #endregion

        #region Properties

        public static EmojiTypeface Typeface { get; private set; }

        public static IEnumerable<Emoji> AllEmoji
            => from g in AllGroups
               from e in g.EmojiList
               select e;

        public static IList<Group> AllGroups { get; private set; }

        public static IDictionary<string, Emoji> LookupByText { get; private set; }
            = new Dictionary<string, Emoji>();
        public static IDictionary<string, Emoji> LookupByName { get; private set; }
            = new Dictionary<string, Emoji>();
        public static IDictionary<string, Emoji> LookupByCNName { get; private set; }
            = new Dictionary<string, Emoji>();

        public static Regex MatchOne { get; private set; }
        public static HashSet<char> MatchStart { get; private set; }
            = new HashSet<char>();

        public static bool EnableZwjRenderingFallback { get; set; } = true;

        public static bool EnableSubPixelRendering { get; set; } = false;

        public static bool EnableWindowsStyleFlags
        {
            get => m_use_custom_flags.GetValueOrDefault(!Typeface.HasFlagGlyphs);
            set => m_use_custom_flags = value;
        }
        private static Dictionary<string, IEnumerable<IEnumerable<Emoji>>> searchCache = new Dictionary<string, IEnumerable<IEnumerable<Emoji>>>();
        #endregion

        /// <summary>
        /// 获取随机Emoji
        /// </summary>
        /// <returns>Emoji</returns>
        public static Emoji GetRandomEmoji()
        {
            Random random = new Random();
            int randomIndex = random.Next(AllEmoji.Count());
            return AllEmoji.ElementAt(randomIndex);
        }

        /// <summary>
        /// 根据Key获取EmojiList
        /// TODO：尚存在问题，部分Emoji无法从AllEmoji中获取到
        /// </summary>
        /// <returns>Emoji</returns>
        public static IEnumerable<IEnumerable<Emoji>> GetValue(string key)
        {
            if (searchCache.TryGetValue(key,out IEnumerable<IEnumerable<Emoji>> result))
            {
                return result;
            }

            var emojiStrs = CNToEmojiCharsData.GetValue(key);
            if (emojiStrs.Count() <= 0) return null;

            var searchEmoji = new List<Emoji>();
            foreach (var emoji in AllEmoji)
            {
                foreach (var es in emojiStrs)
                {
                    if (es.Equals(emoji.Text))
                    {
                        searchEmoji.Add(emoji);
                    }
                    else if(emoji.CNName.Intersect(es).Any() 
                            || String.Join("", emoji.CNNames).Intersect(es).Any() 
                            || emoji.Name.Intersect(es).Any())
                    {
                        searchEmoji.Add(emoji);
                    }

                }
            }
            result =  searchEmoji.Chunk(8);
            if (searchCache.ContainsKey(key))
            {
                searchCache[key] = result;
            }
            else 
            { 
                searchCache.Add(key, result);
            }
            return result;
        }

        public static IEnumerable<IEnumerable<Emoji>> GetEmojisByText(List<string> keys)
        {
            var emojis = new List<Emoji>();
            foreach (var key in keys)
            {
                if(LookupByText.ContainsKey(key))
                    emojis.Add(LookupByText[key]);
            }
            return emojis.Chunk(8);
        }

        public static void ChangeFont(string font_name)
        {
            _fontName = font_name;
            Load(font_name);
        }

        public static void Load()
        {
            Load(null);
        }

        public static void Load(string font_name)
        {
            Typeface = new EmojiTypeface(font_name);
            _fontName = Typeface.ColorTypeface.Name;
            ParseEmojiList();

            // Insert Microsoft’s custom hacker emoji (in reverse order)
            //RegisterEmoji("hacker cat", "🐱\u200d💻", after: "pouting cat");
            //RegisterEmoji("dino cat", "🐱\u200d🐉", after: "pouting cat");
            //RegisterEmoji("ninja cat", "🐱\u200d👤", after: "pouting cat");
            //RegisterEmoji("astro cat", "🐱\u200d🚀", after: "pouting cat");
            //RegisterEmoji("hipster cat", "🐱\u200d👓", after: "pouting cat");
            //RegisterEmoji("stunt cat", "🐱\u200d🏍", after: "pouting cat");

            // Some custom flags that we like to have
            //RegisterEmoji("anarchy flag", "🏴️‍🅰️", after: "transgender-flag");
            //RegisterEmoji("flag: Asturias", "🏴󠁥󠁳󠁡󠁳󠁿", after: "flag-american-samoa");
            //RegisterEmoji("flag: Québec", "🏴󠁣󠁡󠁱󠁣󠁿", after: "flag-qatar");
            //RegisterEmoji("flag: Basque Country", "🏴󠁥󠁳󠁰󠁶󠁿", after: "flag-bosnia-herzegovina");
            //RegisterEmoji("flag: Bretagne", "🏴󠁦󠁲󠁢󠁲󠁥󠁿", after: "flag-brazil");
            //RegisterEmoji("flag: Catalonia", "🏴󠁥󠁳󠁣󠁴󠁿", after: "flag-canada");
        }


        #region Custom emoji sequences
        public static void RegisterEmoji(string name, string sequence, string after)
        {
            if (!LookupByName.TryGetValue(ToColonSyntax(after), out var predecessor))
                predecessor = AllEmoji.Last();

            var entry = new Emoji
            {
                Name = name,
                Text = sequence,
                SubGroup = predecessor.SubGroup,
            };
            var list = predecessor.SubGroup.EmojiList;
            list.Insert(list.IndexOf(predecessor) + 1, entry);

            LookupByName[ToColonSyntax(name)] = entry;
            LookupByText[sequence] = entry;
            MatchStart.Add(sequence[0]);

            m_match_one_string = sequence.Replace("\ufe0f", "\ufe0f?") + "|" + m_match_one_string;
            MatchOne = new Regex("(" + m_match_one_string + ")");
        }
        #endregion

        #region Custom drawings
        public static void RegisterDrawing(string sequence, Drawing dg)
        {
            m_custom_drawings[sequence] = dg;
            EmojiInline.InvalidateCache(sequence);
        }

        public static Drawing GetDrawing(string sequence)
            => m_custom_drawings.TryGetValue(sequence, out var ret) ? ret : null;

        private static Dictionary<string, Drawing> m_custom_drawings
            = new Dictionary<string, Drawing>();
        #endregion


        private static string ToColonSyntax(string s)
            => Regex.Replace(s.Trim().ToLowerInvariant(), "[^a-z0-9]+", "-");
        
        private static void ParseEmojiList()
        {

            #region Regex
            // emoji 组 eg：# group: Smileys & Emotion
            var match_group = new Regex(@"^# group: (.*)");
            // emoji 子组 eg：# subgroup: face-smiling
            var match_subgroup = new Regex(@"^# subgroup: (.*)");
            // 1F938 1F3FF 200D 2640   ; minimally-qualified # 🤸🏿‍♀ E4.0 woman cartwheeling: dark skin tone
            var match_sequence = new Regex(@"^([0-9a-fA-F ]+[0-9a-fA-F]).*; *([-a-z]*) *# [^ ]* (E[0-9.]* )?(.*)");
            // 肤色
            var match_skin_tone = new Regex($"({string.Join("|", SkinToneComponents)})");
            // 发型
            var match_hair_style = new Regex($"({string.Join("|", HairStyleComponents)})");

            #endregion

            Encoding unicode = Encoding.Unicode;
            Encoding ascii = Encoding.ASCII;

            var adult = "(👨|👩)(🏻|🏼|🏽|🏾|🏿)?";
            var child = "(👦|👧|👶)(🏻|🏼|🏽|🏾|🏿)?";
            var match_family = new Regex($"{adult}(\u200d{adult})*(\u200d{child})+");

            var qualified_lut = new Dictionary<string, string>();
            var unqualified_lut = new Dictionary<string, string>();
            var list = new List<Group>();
            var alltext = new List<string>();

            Group current_group = null;
            SubGroup current_subgroup = null;

            var lines = EmojiDescriptionLines();
            foreach (var line in lines)
            {
                #region Group
                var m = match_group.Match(line);
                if (m.Success)
                {
                    current_group = new Group { Name = m.Groups[1].ToString() };
                    list.Add(current_group);
                    continue;
                }
                #endregion

                #region SubGroup

                m = match_subgroup.Match(line);
                if (m.Success)
                {
                    current_subgroup = new SubGroup { Name = m.Groups[1].ToString(), Group = current_group };
                    current_group.SubGroups.Add(current_subgroup);
                    continue;
                }
                #endregion

                m = match_sequence.Match(line);
                if (m.Success)
                {
                    string sequence = m.Groups[1].ToString();
                    string qualified = m.Groups[2].ToString();

                    if (qualified != "fully-qualified")
                        continue;

                    string name = m.Groups[4].ToString();

                    string text = string.Join("", from n in sequence.Split(' ')
                                                  select char.ConvertFromUtf32(Convert.ToInt32(n, 16)));

                    bool has_modifier = false;

                    if (match_family.Match(text).Success)
                    {
                        // 如果这是一个家庭 emoji，不需要将其添加到我们的大正则表达式中，
                        // 因为 match_family 正则表达式已经包含了它。
                        // If this is a family emoji, no need to add it to our big matching
                        // regex, since the match_family regex is already included.
                    }
                    else
                    {
                        // Construct a regex to replace e.g. "🏻" with "(🏻|🏼|🏽|🏾|🏿)" in a big
                        // regex so that we can match all variations of this Emoji even if they are
                        // not in the standard.
                        // 构建一个正则表达式，用于将例如 "🏻" 替换为 "(🏻|🏼|🏽|🏾|🏿)"，
                        // 以便我们可以匹配所有这个 Emoji 的变体，即使它们不在标准中。
                        bool has_nonfirst_modifier = false;
                        var regex_text = match_skin_tone.Replace(
                            match_hair_style.Replace(text, (x) =>
                            {
                                has_modifier = true;
                                has_nonfirst_modifier |= x.Value != HairStyleComponents[0];
                                return match_hair_style.ToString();
                            }), (x) =>
                            {
                                has_modifier = true;
                                has_nonfirst_modifier |= x.Value != SkinToneComponents[0];
                                return match_skin_tone.ToString();
                            });

                        if (!has_nonfirst_modifier)
                            alltext.Add(has_modifier ? regex_text : text);
                    }

                    // 如果已经有一个不同资格版本的此字符，跳过它。
                    // If there is already a differently-qualified version of this character, skip it.
                    // FIXME: this only works well if fully-qualified appears first in the list.
                    // 改为如果不是 fully-qualified ，则跳过
                    var unqualified = text.Replace("\ufe0f", "");

                    /*if (qualified_lut.ContainsKey(unqualified) && qualified != "fully-qualified")
                        continue;*/

                    qualified_lut[unqualified] = text;

                    if (text == "1⃣️")
                    {
                        int d = 0;
                    }
                    var cnname = CNEmojiData.CNEmojiData.GetValue(text, "name_i18n");
                    var emojiChars = EmojiCharToCNKeysData.GetValue(text);

                    var emoji = new Emoji
                    {
                        Name = name,
                        CNName = cnname,
                        Text = text,
                        SubGroup = current_subgroup,
                        Renderable = Typeface.CanRender(text),
                        CNNames = emojiChars,
                    };
                    // FIXME: this prevents LookupByText from working with the unqualified version
                    LookupByText[text] = emoji;
                    LookupByName[ToColonSyntax(name)] = emoji;
                    if (!LookupByCNName.ContainsKey(text)) LookupByCNName.Add(text, emoji);
                    MatchStart.Add(text[0]);

                    // Get the left part of the name and check whether we’re a variation of an existing
                    // emoji. If so, append to that emoji. Otherwise, add to current subgroup.
                    // FIXME: does not work properly because variations can appear before the generic emoji
                    if (name.Contains(":") && LookupByName.TryGetValue(ToColonSyntax(name.Split(':')[0]), out var parent_emoji))
                    {
                        if (parent_emoji.VariationList.Count == 0)
                            parent_emoji.VariationList.Add(parent_emoji);
                        parent_emoji.VariationList.Add(emoji);
                    }
                    else
                        current_subgroup.EmojiList.Add(emoji);
                }
            }

            // Remove the Component group. Not sure we want to have the skin tones in the picker.
            list.RemoveAll(g => g.Name == "Component");
            AllGroups = list;

            // Make U+fe0f optional in the regex so that we can match any combination.
            // FIXME: this is the starting point to implement variation selectors.
            var sortedtext = alltext.OrderByDescending(x => x.Length);
            var match_other = string.Join("|", sortedtext)
                                    .Replace("*", "[*]")
                                    .Replace("\ufe0f", "\ufe0f?");

            // Build a regex that matches any Emoji
            m_match_one_string = match_family.ToString() + "|" + match_other;
            MatchOne = new Regex("(" + m_match_one_string + ")");
        }

        private static IEnumerable<string> EmojiDescriptionLines()
        {
            using (var sr = new GZipResourceStream("emoji-test.txt.gz"))
                return sr.ReadToEnd().Split('\r', '\n');
        }
    }
}

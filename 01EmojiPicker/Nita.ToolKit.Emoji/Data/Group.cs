using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.Emoji.Data
{
    /// <summary>
    /// Emoji group class.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Emoji group name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Emoji group Chinese name.
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// Emoji group Icon.
        /// </summary>
        public string Icon => SubGroups.FirstOrDefault()?.EmojiList.FirstOrDefault()?.Text;
        /// <summary>
        ///  Emoji sub group Icon.
        /// </summary>
        public IList<SubGroup> SubGroups { get; } = new List<SubGroup>();
        /// <summary>
        /// Emoji count in group.
        /// </summary>
        public int EmojiCount => SubGroups.Select(s => s.EmojiList.Count).Sum();
        /// <summary>
        /// Emoji chunk list.
        /// </summary>
        public IEnumerable<IEnumerable<Emoji>> EmojiChunkList => EmojiList.Chunk(8);

        public IEnumerable<Emoji> EmojiList
            => from s in SubGroups
               from e in s.EmojiList
               select e;
    }
}

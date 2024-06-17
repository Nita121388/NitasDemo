using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Nita.ToolKit.Emoji.Data
{
    /// <summary>
    /// Emoji sub group
    /// </summary>
    public class SubGroup
    {
        public string Name { get; set; }
        public string CNName { get; set; }

        public Group Group;

        public IList<Emoji> EmojiList { get; } = new List<Emoji>();
    }

}

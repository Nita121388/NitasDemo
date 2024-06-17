
namespace Nita.ToolKit.Emoji.Data
{
    public class Emoji
    {
        /// <summary>
        /// Emoji的英文名称、形容
        /// "smiling face with smiling eyes"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  Emoji的中文名称、形容
        /// 微笑的脸和微笑的眼睛
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// Emoji相关的中文名称集合
        /// </summary>
        public List<string> CNNames { get; set; }
        public string CNNamesString
        {
            get { return String.Join(", ", CNNames); }
        }

        /// <summary>
        ///  Emoji的Unicode编码
        /// 😊
        /// </summary>
        public string Text { get; set; }
        public bool Renderable { get; set; }
        /// <summary>
        /// Emoji是否有变体
        /// </summary>
        public bool HasVariations => VariationList.Count > 0;

        public Group Group => SubGroup.Group;

        public SubGroup SubGroup;
        public IList<Emoji> VariationList { get; } = new List<Emoji>();
    }
}

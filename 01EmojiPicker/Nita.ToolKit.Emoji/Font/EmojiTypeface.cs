using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Typography.TextLayout;

namespace Nita.ToolKit.Emoji.Font
{
    /// <summary>
    /// The EmojiTypeface class exposes layout and rendering primitives from a
    /// ColorTypeface.
    ///  EmojiTypeface类从ColorTypeface中公开布局和渲染原语。
    /// In the future this object may use several ColorTypeFace for better coverage.
    /// 未来，这个对象可能会使用多个ColorTypeFace以获得更好的覆盖范围。
    /// </summary>
    public class EmojiTypeface
    {
        public EmojiTypeface(string font_name = null)
            => m_fonts.Add(new ColorTypeface(font_name));

        public double Height
            => (double)m_fonts.FirstOrDefault()?.Height;

        public double Baseline
            => (double)m_fonts.FirstOrDefault()?.Baseline;

        public bool CanRender(string s)
            => m_fonts[0].CanRender(s);

        public double GetScale(double point_size)
            => m_fonts[0].GetScale(point_size);

        public ushort ZwjGlyph
            => m_fonts[0].ZwjGlyph;
        internal ColorTypeface ColorTypeface
            => m_fonts[0];

        public bool HasFlagGlyphs
            => m_fonts[0].HasFlagGlyphs;

        public bool HasWin11Emoji
            => m_fonts[0].HasWin11Emoji;
        public bool OrtherEmoji
            => m_fonts[0].OrtherEmoji;

        public IEnumerable<ushort> MakeGlyphIndexList(string s)
            => MakeGlyphPlanList(s).Select(x => x.glyphIndex);

        public IList<UnscaledGlyphPlan> MakeGlyphPlanList(string s)
        {
            if (!m_cache.TryGetValue(s, out var ret))
                m_cache[s] = ret = m_fonts[0].StringToGlyphPlans(s).ToList();
            return ret;
        }

        public IEnumerable<(GlyphRun, Brush)> DrawGlyph(ushort gid)
            => m_fonts[0].DrawGlyph(gid);

        /// <summary>
        /// A cache of GlyphPlanList objects, indexed by source strings. Should
        /// remain pretty lightweight because they are small objects.
        /// FIXME: measure how many cache hits we actually benefit from
        /// </summary>
        private readonly IDictionary<string, IList<UnscaledGlyphPlan>> m_cache
            = new Dictionary<string, IList<UnscaledGlyphPlan>>();

        private readonly IList<ColorTypeface> m_fonts = new List<ColorTypeface>();
    }

}

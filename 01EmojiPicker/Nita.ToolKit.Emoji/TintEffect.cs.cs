using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;

namespace Nita.ToolKit.Emoji
{

    public class TintEffect : ShaderEffect
    {
        static TintEffect()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("TintEffect.ps");
            m_shader = new PixelShader();
            m_shader.SetStreamSource(stream);
        }

        public TintEffect()
        {
            PixelShader = m_shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(TintProperty);
        }

        public static readonly DependencyProperty TintProperty =
            DependencyProperty.Register(nameof(Tint), typeof(Color), typeof(TintEffect),
                new UIPropertyMetadata(Colors.Red, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(TintEffect), 0);

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public Color Tint
        {
            get => (Color)GetValue(TintProperty);
            set => SetValue(TintProperty, value);
        }

        private static PixelShader m_shader;
    }
}

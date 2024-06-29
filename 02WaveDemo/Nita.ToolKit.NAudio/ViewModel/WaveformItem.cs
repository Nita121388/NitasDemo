using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Nita.ToolKit.NAudio.ViewModel
{
    public class WaveformItem : ObservableObject
    {
        private Brush _color;
        public Brush Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        private double _position;
        public double Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }
        private double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }
        private double _left;
        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }
        private double _top;
        public double Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

    }
}

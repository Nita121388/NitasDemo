using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nita.ToolKit.NAudio.ViewModel
{
    public class WaveformViewModel : ObservableObject
    {
        private ObservableCollection<WaveformItem> _waveformItems;
        public ObservableCollection<WaveformItem> WaveformItems
        {
            get { return _waveformItems; }
            set { SetProperty(ref _waveformItems, value); }
        }

        private int _currentPlayIndex;
        public int CurrentPlayIndex
        {
            get { return _currentPlayIndex; }
            set
            {
                if (SetProperty(ref _currentPlayIndex, value))
                {
                    UpdateWaveformColors();
                }
            }
        }

        public WaveformViewModel()
        {
            WaveformItems = new ObservableCollection<WaveformItem>();
        }

        private void UpdateWaveformColors()
        {
            for (int i = 0; i < WaveformItems.Count; i++)
            {
                if (i < CurrentPlayIndex)
                {
                    WaveformItems[i].Color = Brushes.Gray; 
                }
                else if (i == CurrentPlayIndex)
                {
                    WaveformItems[i].Color = Brushes.Red; 
                }
                else
                {
                    WaveformItems[i].Color = Brushes.Blue;
                }
            }
        }
    }
}

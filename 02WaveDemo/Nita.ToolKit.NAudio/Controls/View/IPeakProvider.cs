using NAudio.Wave;

namespace Nita.ToolKit.NAudio.Controls.View
{
    public interface IPeakProvider
    {
        void Init(ISampleProvider reader, int samplesPerPixel);
        PeakInfo GetNextPeak();
    }
}
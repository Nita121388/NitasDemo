using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.NAudio.Entity
{
    public enum PeakCalculationStrategy
    {
        Max_Absolute_Value,
        Max_Rms_Value,
        Sampled_Peaks,
        Average
    }
}

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Threading.Tasks;

namespace NitasTool.Helper
{
    public class StatusBarHelper
    {
        public static async void ShowStatusBarText(IAsyncServiceProvider serviceProvider, string text)
        {
            IVsStatusbar statusBar = (IVsStatusbar)await serviceProvider.GetServiceAsync(typeof(SVsStatusbar));

            // Make sure the status bar is not frozen
            int frozen;
            statusBar.IsFrozen(out frozen);

            if (frozen != 0)
            {
                statusBar.FreezeOutput(0);
            }

            // Set the status bar text and make its display static.
            statusBar.SetText(text);
            // Freeze the status bar.
            statusBar.FreezeOutput(1);

            // Wait for 5 seconds
            await Task.Delay(5000);

            // Clear the status bar text.
            statusBar.FreezeOutput(0);
            statusBar.Clear();
        }
    }
}

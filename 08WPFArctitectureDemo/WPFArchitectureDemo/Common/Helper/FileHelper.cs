using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace WPFArchitectureDemo.UI.Common.Helper
{
    internal class FileHelper
    {
        public static string SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }

        public static string SelectFolder()
        {
            var dialog = new VistaFolderBrowserDialog();
            if ((bool)dialog.ShowDialog())
            {
                return dialog.SelectedPath;
            }
            return "";
        }
    }
}

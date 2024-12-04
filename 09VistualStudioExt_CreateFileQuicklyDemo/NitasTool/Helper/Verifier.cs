using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using NitasTool.Entity;
using NitasTool.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitasTool.Helper
{
    internal class Verifier
    {
        public bool IsValid(SelectTextType selectedContentType,string selectedText, AsyncPackage package, string title)
        {
            if (selectedContentType == SelectTextType.Word)
            {
                var validationResult = FileNameValidator.IsValidFileName(selectedText);

                if (!validationResult.IsValid)
                {
                    string errorMessage = validationResult.ErrorMessage;
                    VsShellUtilities.ShowMessageBox(
                        package,
                        "No class name selected",
                        title,
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return false;
                }
            }
            return true;
        }
    }
}

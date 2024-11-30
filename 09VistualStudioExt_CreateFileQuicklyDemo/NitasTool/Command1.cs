using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NitasTool.Helper;
using NitasTool.Validator;
using NitasTool.Views;
using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace NitasTool
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command1
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("cd2fada3-6354-43b0-8980-f0eee618e72e");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Command1(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command1 Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Command1's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new Command1(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            SelectClassNameToNewFile();
        }

        private async void SelectClassNameToNewFile()
        {
            try
            {
                string title = "Selelect  To Create New File";

                var result = await ServiceProvider.GetServiceAsync(typeof(DTE));
                var dte = (DTE)result;
                if (result == null || dte == null)
                {
                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        "DTE object is null",
                        title,
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                    return;
                }

                var selectedText = CodeHelper.GetSelectedText(dte.ActiveDocument);

                var validationResult = FileNameValidator.IsValidFileName(selectedText);

                if (!validationResult.IsValid)
                {
                    string errorMessage = validationResult.ErrorMessage;
                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        "No class name selected",
                        title,
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return;
                }

                var SelectionAnalyzer = new CodeAnalyzer(dte);
                SelectionAnalyzer.CreateToNewClassFile(selectedText, 
                    HandleExistedClassFile, 
                    HandleBerforeCrrateNewClassFile,
                    HandleAfterNewClassFile);
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    ex.Message,
                    "Error",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        public UserResponse HandleExistedClassFile()
        {
            string message = "File already exists! \r\n " +
                "Do you want to overwrite it ? \r\n " +
                "Or auto rename and create a new one? \r\n" +
                " or cancel?";
            string title = "File Already Exists";

            var result = new NitasMessageBox(title, message).ShowDialog();
            switch (result) 
            { 
                case DialogResult.Yes: 
                    return UserResponse.Overwrite; 
                case DialogResult.No: 
                    return UserResponse.AutoRename; 
                case DialogResult.Cancel: 
                    return UserResponse.Cancel; 
                default: 
                    return UserResponse.Cancel; 
            }
        }
        public void HandleAfterNewClassFile()
        {
            StatusBarHelper.ShowStatusBarText(ServiceProvider, $"✅ New  File created!");
        }
        public void HandleBerforeCrrateNewClassFile()
        {
            StatusBarHelper.ShowStatusBarText(ServiceProvider, $"⏳New  File creating...");
        }
        public enum UserResponse
        {
            Overwrite,
            AutoRename,
            Cancel
        }
    }
}

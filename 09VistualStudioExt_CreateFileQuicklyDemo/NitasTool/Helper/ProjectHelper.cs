using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NitasTool.Helper
{
    /// <summary>
    /// 项目帮助
    /// </summary>
    internal class ProjectHelper
    {

        /// <summary>
        /// 异步创建文件并添加到项目中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="classCode"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static async Task CreateAndAddFileAsync(string filePath, string classCode, Project project, bool isOpen = true)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(classCode);
            }

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ProjectItem newItem = project.ProjectItems.AddFromFile(filePath);

            if (isOpen)
            {
                newItem.Open(EnvDTE.Constants.vsViewKindTextView).Visible = true;
            }
        }
        
        /// <summary>
        /// 异步创建文件夹并添加到项目中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="classCode"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static async Task CreateAndAddFolderAsync(string folderPath, Project project, bool isOpen = false)
        {
            Directory.CreateDirectory(folderPath);
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ProjectItem newItem = project.ProjectItems.AddFolder(folderPath);
        }

    }
}

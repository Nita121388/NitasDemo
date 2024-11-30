using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NitasTool.Command1;

namespace NitasTool.Helper
{
    internal class FileHelper
    {
        public static async Task CreateAndAddFileAsync(string filePath, string classCode, Project project)
        {
            // Create new file with the class code asynchronously
             File.WriteAllText(filePath, classCode);

            // Add the new file to the project
            ProjectItem newItem = project.ProjectItems.AddFromFile(filePath);

            // Open the new file in the text editor
            newItem.Open(EnvDTE.Constants.vsViewKindTextView).Visible = true;
        }

        public static string GetUniqueFilePath(string projectPath, string baseFileName)
        {
            int counter = 1;
            string newFilePath;
            do
            {
                newFilePath = Path.Combine(projectPath, $"{baseFileName}_{counter}.cs");
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }

        /// <summary>
        /// 异步创建新项目文件。
        /// </summary>
        /// <param name="document">当前文档对象。</param>
        /// <param name="codeContext">代码上下文，用于生成类代码。</param>
        /// <param name="selectedName">用户选择的新文件名。</param>
        /// <param name="handleExistedClassFile">处理已存在类文件的回调函数，返回用户响应。</param>
        /// <param name="handleBeforeCreateNewClassFile">在创建新类文件之前执行的操作。</param>
        /// <param name="handleAfterNewClassFile">在新类文件创建后执行的操作。</param>
        /// <returns>一个表示异步操作的任务。</returns>
        public static async Task CreateNewItemAsync(
            Document document,
            CodeContext codeContext,
            string selectedName,
            Func<UserResponse> handleExistedClassFile,
            Action handleBeforeCreateNewClassFile,
            Action handleAfterNewClassFile)
        {
            var classCode = CodeHelper.AddNamespaceToCode(codeContext);

            var project = document.ProjectItem.ContainingProject;
            var currentDocumentPath = Path.GetDirectoryName(document.FullName);
            var newFilePath = Path.Combine(currentDocumentPath, selectedName + ".cs");

            bool isCreated = false;
            if (File.Exists(newFilePath))
            {
                var userResponse = handleExistedClassFile();

                switch (userResponse)
                {
                    case UserResponse.Overwrite:
                        handleBeforeCreateNewClassFile();
                        await CreateAndAddFileAsync(newFilePath, classCode, project);
                        isCreated = true;
                        break;
                    case UserResponse.AutoRename:
                        newFilePath = FileHelper.GetUniqueFilePath(currentDocumentPath, selectedName);
                        handleBeforeCreateNewClassFile();
                        await CreateAndAddFileAsync(newFilePath, classCode, project);
                        isCreated = true;
                        break;
                    case UserResponse.Cancel:
                        return;
                }
            }
            else
            {
                handleBeforeCreateNewClassFile();
                await CreateAndAddFileAsync(newFilePath, classCode, project);
                isCreated = true;
            }

            if (isCreated)
            {
                handleAfterNewClassFile();
            }
        }
    }
}

using EnvDTE;
using NitasTool.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NitasTool.Helper
{

    internal class FileHelper
    {
        public static async Task CreateNewItemAsyncByCreateItemInfoAsync(CreateItemInfo createItemInfo)
        {
            var project = createItemInfo.Document.ProjectItem.ContainingProject;
            var currentDocumentPath = Path.GetDirectoryName(createItemInfo.Document.FullName);
            var currentDocumentExtension = Path.GetExtension(createItemInfo.Document.FullName);
            var handleExistedClassFile = createItemInfo.HandleExistedClassFile;
            var handleBeforeCreateNewClassFile = createItemInfo.HandleBeforeCreateNewClassFile;
            var handleAfterNewClassFile = createItemInfo.HandleAfterNewClassFile;
            var selectedName = createItemInfo.CodeContext?.Name;
            if (createItemInfo.CreateItemType == CreateItemType.File)
            {
                var classCode = CodeHelper.AddNamespaceToCode(createItemInfo.CodeContext);
                var filePath = Path.Combine(currentDocumentPath, selectedName + currentDocumentExtension);
                await CreateNewItemAsync(project,
                                            currentDocumentPath,
                                            filePath,
                                            classCode,
                                            selectedName,
                                            handleExistedClassFile,
                                            handleBeforeCreateNewClassFile,
                                            handleAfterNewClassFile);
            }
            else if (createItemInfo.CreateItemType == CreateItemType.EmptyFile)
            {
                var filePath = Path.Combine(currentDocumentPath, selectedName);
                await CreateNewItemAsync(project,
                                            currentDocumentPath,
                                            filePath,
                                            "",
                                            selectedName,
                                            handleExistedClassFile,
                                            handleBeforeCreateNewClassFile,
                                            handleAfterNewClassFile);
            }
            else if (createItemInfo.CreateItemType == CreateItemType.Folder)
            {
                foreach (var folder in createItemInfo.Folder.Folders)
                {
                    await CreateNewItemAsyncByFolderAsync(createItemInfo, folder);
                }
                if (createItemInfo.Folder.Files.Count > 0)
                {
                    await CreateNewItemByFilesAsync(project, currentDocumentPath, createItemInfo.Folder.Files, handleExistedClassFile, handleBeforeCreateNewClassFile, handleAfterNewClassFile);
                }
            }
            handleAfterNewClassFile();
        }

        #region private methods
        private static async Task CreateNewItemAsyncByFolderAsync(CreateItemInfo createItemInfo, Folder folder)
        {
            var project = createItemInfo.Document.ProjectItem.ContainingProject;
            var currentDocumentPath = Path.GetDirectoryName(createItemInfo.Document.FullName);
            var currentDocumentExtension = Path.GetExtension(createItemInfo.Document.FullName);
            var handleExistedClassFile = createItemInfo.HandleExistedClassFile;
            var handleBeforeCreateNewClassFile = createItemInfo.HandleBeforeCreateNewClassFile;
            var handleAfterNewClassFile = createItemInfo.HandleAfterNewClassFile;
            var selectedName = createItemInfo.CodeContext?.Name;


            await CreateNewFolderItemAsync(project, currentDocumentPath, folder.FolderPath,
                                    handleBeforeCreateNewClassFile,
                                    handleAfterNewClassFile);

            if (folder.Files.Count > 0)
            {
                var folderPath = Path.Combine(currentDocumentPath, folder.FolderPath);
                await CreateNewItemByFilesAsync(project, folderPath, folder.Files, handleExistedClassFile, handleBeforeCreateNewClassFile, handleAfterNewClassFile);
            }

            foreach (var f in folder.Folders)
            {
                await CreateNewItemAsyncByFolderAsync(createItemInfo, f);
            }
            handleAfterNewClassFile();
        }


        /// <summary>
        /// 创建新文件夹
        /// </summary>
        /// <param name="project"></param>
        /// <param name="currentDocumentPath"></param>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="selectedName"></param>
        /// <param name="handleExistedClassFile"></param>
        /// <param name="handleBeforeCreateNewClassFile"></param>
        /// <param name="handleAfterNewClassFile"></param>
        /// <returns></returns>
        private static async Task<string> CreateNewFolderItemAsync(
            Project project,
            string currentDocumentPath,
            string folderName,
            Action handleBeforeCreateNewClassFile,
            Action handleAfterNewClassFile)
        {
            var folderPath = Path.Combine(currentDocumentPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                await ProjectHelper.CreateAndAddFolderAsync(folderPath, project);
            }
            else
            {
                await ProjectHelper.CreateAndAddFolderAsync(folderPath, project);
            }

            handleAfterNewClassFile();
            return folderPath;
        }

        /// <summary>
        /// 创建新文件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="currentDocumentPath"></param>
        /// <param name="Files"></param>
        /// <param name="handleExistedClassFile"></param>
        /// <param name="handleBeforeCreateNewClassFile"></param>
        /// <param name="handleAfterNewClassFile"></param>
        /// <returns></returns>
        private static async Task CreateNewItemByFilesAsync(
            Project project,
            string currentDocumentPath,
            List<string> Files,
            Func<string, UserResponse> handleExistedClassFile,
            Action handleBeforeCreateNewClassFile,
            Action handleAfterNewClassFile)
        {

            for (int i = 0; i < Files.Count; i++)
            {
                var newFilePath = Path.Combine(currentDocumentPath, Files[i]);

                bool isCreated = false;
                if (File.Exists(newFilePath))
                {
                    var userResponse = handleExistedClassFile(newFilePath);

                    switch (userResponse)
                    {
                        case UserResponse.Overwrite:
                            handleBeforeCreateNewClassFile();
                            await ProjectHelper.CreateAndAddFileAsync(newFilePath, "", project);
                            isCreated = true;
                            break;
                        case UserResponse.AutoRename:
                            newFilePath = FileHelper.GetUniqueFilePath(currentDocumentPath, Files[i]);
                            handleBeforeCreateNewClassFile();
                            await ProjectHelper.CreateAndAddFileAsync(newFilePath, "", project);
                            isCreated = true;
                            break;
                        case UserResponse.Cancel:
                            continue;
                    }
                }
                else
                {
                    handleBeforeCreateNewClassFile();
                    await ProjectHelper.CreateAndAddFileAsync(newFilePath, "", project);
                    isCreated = true;
                }

                if (isCreated)
                {
                    handleAfterNewClassFile();
                }
            }

        }

        /// <summary>
        /// 创建新文件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="currentDocumentPath"></param>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="selectedName"></param>
        /// <param name="handleExistedClassFile"></param>
        /// <param name="handleBeforeCreateNewClassFile"></param>
        /// <param name="handleAfterNewClassFile"></param>
        /// <returns></returns>
        private static async Task CreateNewItemAsync(
            Project project,
            string currentDocumentPath,
            string filePath,
            string content,
            string selectedName,
            Func<string, UserResponse> handleExistedClassFile,
            Action handleBeforeCreateNewClassFile,
            Action handleAfterNewClassFile)
        {

            if (File.Exists(filePath))
            {
                switch (handleExistedClassFile(filePath))
                {
                    case UserResponse.Overwrite:
                        await CreateNewItemAsync(project, filePath, content, handleBeforeCreateNewClassFile);
                        break;
                    case UserResponse.AutoRename:
                        filePath = FileHelper.GetUniqueFilePath(currentDocumentPath, selectedName);
                        await CreateNewItemAsync(project, filePath, content, handleBeforeCreateNewClassFile);
                        break;
                    case UserResponse.Cancel:
                        return;
                }
            }
            else
            {
                await CreateNewItemAsync(project, filePath, content, handleBeforeCreateNewClassFile);
            }

            handleAfterNewClassFile();
        }

        /// <summary>
        /// 创建新文件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="handleBeforeCreateNewClassFile"></param>
        /// <returns></returns>
        private static async Task CreateNewItemAsync(
            Project project, 
            string filePath, 
            string content,
            Action handleBeforeCreateNewClassFile)
        {
            handleBeforeCreateNewClassFile();
            await ProjectHelper.CreateAndAddFileAsync(filePath, content, project);
        }


        /// <summary>
        /// 获取一个唯一的文件路径。
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="baseFileName"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetUniqueFilePath(string projectPath, string baseFileName, string fileExtension = ".cs")
        {
            if (baseFileName.EndsWith(fileExtension))
            {
                baseFileName = baseFileName.Remove(baseFileName.Length - fileExtension.Length, fileExtension.Length);
            }

            int counter = 1;
            string newFilePath;
            do
            {
                newFilePath = Path.Combine(projectPath, $"{baseFileName}_{counter}{fileExtension}");
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }
        #endregion
    }
}

using EnvDTE;
using Microsoft.VisualStudio.Experimentation;
using NitasTool.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NitasTool.Entity
{
    public class CreateItemInfo
    {
        public CreateItemType CreateItemType { get; set; }
        public Project Project { get; set; }
        public Document Document { get; set; }
        public string SelectedText { get; set; }
        public string CurrentDocumentName { get; set; }
        public string DoucmentFilePath { get; set; }
        public string CurrentDocumentPath { get; set; }
        public string CurrentDocumentExtension { get; set; }
        public CodeContext CodeContext { get; set; }
        public Folder Folder { get; set; }
        public Func<string, UserResponse> HandleExistedClassFile { get; set; }
        public Action HandleBeforeCreateNewClassFile { get; set; }
        public Action HandleAfterNewClassFile { get; set; }

        public CreateItemInfo(string selectedText,Document document,Func<string,UserResponse> handleExistedClassFile,Action handleBeforeCreateNewClassFile,Action handleAfterNewClassFile)
        {
            SelectedText = selectedText;
            var selectedContentType = CodeHelper.ContentType(selectedText);
            if (selectedContentType == SelectTextType.Word)
            {
                CreateItemType = CreateItemType.File;
            }
            else if (selectedContentType == SelectTextType.File)
            {
                CreateItemType = CreateItemType.EmptyFile;
            }
            else if (selectedContentType == SelectTextType.Folder)
            {
                CreateItemType = CreateItemType.Folder;
            }

            Document = document;
            Project = document.ProjectItem.ContainingProject;
            DoucmentFilePath = document.FullName;
            CurrentDocumentName = Path.GetFileName(DoucmentFilePath);
            CurrentDocumentPath = Path.GetDirectoryName(DoucmentFilePath);
            CurrentDocumentExtension = Path.GetExtension(DoucmentFilePath);

            if (CreateItemType == CreateItemType.File)
            {
                CodeContext = CodeHelper.GetCodeContext(Document, selectedText);
            }

            if (CodeContext != null
                && CodeContext.ElementContext.Count() == 0)
            {
                CreateItemType = CreateItemType.Folder;
            }

            if( CreateItemType == CreateItemType.EmptyFile)
            {
                CodeContext = new CodeContext();
                CodeContext.Name = selectedText.Trim();
            }

            if (CreateItemType == CreateItemType.Folder)
            {
                Folder = CodeHelper.GetFolder(selectedText);
            }

            HandleExistedClassFile = handleExistedClassFile;
            HandleBeforeCreateNewClassFile = handleBeforeCreateNewClassFile;
            HandleAfterNewClassFile = handleAfterNewClassFile;

        }
    }
}

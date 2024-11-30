using EnvDTE;
using System;
using static NitasTool.Command1;
using NitasTool.Helper;

namespace NitasTool
{
    internal class CodeAnalyzer
    {
        private readonly DTE _dte; 
        public CodeAnalyzer(DTE dte) 
        { 
            _dte = dte;
        }

        /// <summary>
        /// 获取当前选中的文本
        /// </summary>
        /// <returns></returns>
        public string GetSelectedText()
        {
            var selection = (TextSelection)_dte.ActiveDocument.Selection;
            var selectedText = selection.Text;

            if (string.IsNullOrEmpty(selectedText))
            {
                // 如果没有选中的文本，则获取当前光标所在位置的单词
                var activePoint = selection.ActivePoint;
                var lineText = activePoint.CreateEditPoint().GetText(activePoint.LineLength);
                int start = activePoint.AbsoluteCharOffset - 1;
                int end = start;

                // 找到当前单词的起始和结束位置
                while (start > 0 && char.IsLetterOrDigit(lineText[start]))
                {
                    start--;
                }
                while (end < lineText.Length && char.IsLetterOrDigit(lineText[end]))
                {
                    end++;
                }

                // 提取单词
                selectedText = lineText.Substring(start, end - start).Trim();
            }

            return selectedText;
        }

        /// <summary>
        /// 获取当前选中的单词
        /// </summary>
        /// <param name="selectedName"></param>
        /// <param name="HandleExistedClassFile"></param>
        /// <param name="HandleAfterNewClassFile"></param>
        public void CreateToNewClassFile(string selectedName, Func<UserResponse> HandleExistedClassFile,
            Action HandleBerforeCrrateNewClassFile,Action HandleAfterNewClassFile )
        {
            var activeDocument = _dte.ActiveDocument;

            //get code context
            var codeContext = CodeHelper.GetCodeContext(activeDocument, selectedName);

            //create new file by context
            FileHelper.CreateNewItemAsync(activeDocument,codeContext, selectedName, HandleExistedClassFile, HandleBerforeCrrateNewClassFile,HandleAfterNewClassFile);
        }
    }
}

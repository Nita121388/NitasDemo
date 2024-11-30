using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Linq;
using System.Text;

namespace NitasTool.Helper
{
    public class CodeHelper
    {

        /// <summary>
        /// 获取当前选中的文本
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedText(Document document)
        {
            var selection = (TextSelection)document.Selection;
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
        /// 给代码补充命名空间
        /// </summary>
        /// <param name="codeContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string AddNamespaceToCode(CodeContext codeContext)
        {
            if (string.IsNullOrWhiteSpace(codeContext.NameSpace))
            {
                throw new ArgumentException("Namespace name cannot be null or empty.", nameof(codeContext.NameSpace));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {codeContext.NameSpace}");
            sb.AppendLine("{");

            string indent = "\t";
            //string indentForClassCode = indent + "\t"; // 双缩进用于类内部的代码

            string[] lines = codeContext.ElementContext;
            bool firstLine = true;
            foreach (string line in lines)
            {
                if (firstLine)
                {
                    // 类定义开始行，只添加单缩进
                    sb.Append(indent).AppendLine(line);
                }
                else
                {
                    // 其他行
                    sb.AppendLine(line);
                }
                firstLine = false;
            }

            // end
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// element是否支持独立成文件
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsCanBeToCreateFile(CodeElement element)
        {
            if (element.Kind == vsCMElement.vsCMElementClass
                || element.Kind == vsCMElement.vsCMElementEnum
                || element.Kind == vsCMElement.vsCMElementInterface
                || element.Kind == vsCMElement.vsCMElementStruct
                  )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定起始点和结束点之间的文本中间内容，并按行分割成字符串数组。
        /// </summary>
        /// <param name="startPoint">文本起始点。</param>
        /// <param name="endPoint">文本结束点。</param>
        /// <returns>包含中间文本内容（按行分割）的字符串数组。</returns>
        public static string[] GetMiddleContextLines(TextPoint startPoint, TextPoint endPoint)
        {
            // 使用起始点的编辑点获取起始点到结束点之间的文本内容
            string textContent = startPoint.CreateEditPoint().GetText(endPoint);

            // 按行分割
            return textContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// 获取选中代码的上下文
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="selectedName">选中的名称</param>
        /// <returns>CodeContext对象，包含代码上下文信息</returns>
        public static CodeContext GetCodeContext(Document document, string selectedName)
        {
            // 获取文档的代码模型
            var codeModel = document.ProjectItem.FileCodeModel;
            var codeContext = new CodeContext();

            TextPoint lastElementEndPoint = null;
            TextPoint currentElementEndPoint = null;
            var codeElements = codeModel.CodeElements as CodeElements;
            bool findSelected = false;

            // 遍历代码元素
            foreach (CodeElement element in codeElements)
            {
                // 只处理命名空间元素
                if (element.Kind != vsCMElement.vsCMElementNamespace)
                    continue;

                var codeNamespace = element as CodeNamespace;
                lastElementEndPoint = element.StartPoint;
                codeContext.NameSpace = codeNamespace.FullName;

                // 遍历命名空间内的子元素
                foreach (CodeElement childElement in element.Children)
                {
                    // 如果找到选中的名称并且支持单独创建为文件
                    if (childElement.Name == selectedName && IsCanBeToCreateFile(childElement))
                    {
                        codeContext.Name = childElement.Name;
                        currentElementEndPoint = childElement.EndPoint;
                        findSelected = true;
                        break;
                    }
                    lastElementEndPoint = childElement.EndPoint;
                }
                if (findSelected) break;
            }

            // 获取选中元素之间的代码行
            string[] codeLines = new string[0];
            if (lastElementEndPoint != null && currentElementEndPoint != null)
            {
                codeLines = GetMiddleContextLines(lastElementEndPoint, currentElementEndPoint);
            }

            // 处理代码行，去除多余的花括号
            if (codeLines.Length > 0)
            {
                var temp = codeLines.Skip(1);
                if (!string.IsNullOrEmpty(temp.First()) && temp.First().Trim().StartsWith("{"))
                {
                    codeLines = temp.Skip(1).ToArray();
                }
                else if (temp != null)
                {
                    codeLines = temp.ToArray();
                }
            }
            codeContext.ElementContext = codeLines;
            return codeContext;
        }
    }
    public class CodeContext
    {
        public string[] ElementContext { get; set; }
        public string NameSpace { get; set; }
        public string Name { get; set; }
    }
}

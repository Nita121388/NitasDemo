using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitasTool.Validator
{

    public class FileNameValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class FileNameValidator
    {
        // 定义不允许的字符
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        public static FileNameValidationResult IsValidFileName(string fileName)
        {
            var result = new FileNameValidationResult { IsValid = true };

            // 检查文件名是否为空
            if (string.IsNullOrEmpty(fileName))
            {
                result.IsValid = false;
                result.ErrorMessage = "文件名不能为空。";
                return result;
            }

            // 检查文件名长度
            if (fileName.Length > 255)
            {
                result.IsValid = false;
                result.ErrorMessage = "文件名长度不能超过 255 个字符。";
                return result;
            }

            // 检查文件名中是否包含不允许的字符
            foreach (var ch in InvalidFileNameChars)
            {
                if (fileName.Contains(ch))
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"文件名不能包含非法字符: {ch}";
                    return result;
                }
            }

            // 检查文件名是否以空格或句点开始或结束
            if (fileName.StartsWith(" ") || fileName.EndsWith(" ") || fileName.StartsWith(".") || fileName.EndsWith("."))
            {
                result.IsValid = false;
                result.ErrorMessage = "文件名不能以空格或句点开始或结束。";
                return result;
            }

            // 如果所有检查都通过，文件名是有效的
            result.ErrorMessage = "文件名有效。";
            return result;
        }

    }
}

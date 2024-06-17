using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.Emoji.Util
{
    internal class AssemblyResourceHelper
    {
        public static Stream Get(string resourcePath,string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = resourcePath + resourceName;

            // 检查资源是否存在
            if (!assembly.GetManifestResourceNames().Contains(path))
            {
                throw new ArgumentException($"Resource '{resourceName}' not found in the assembly.");
            }
                return assembly.GetManifestResourceStream(path);
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace NitasTool.Entity
{
    public class Folder
    { 
        public string Name { get; set; }
        public string FolderPath
        {
            get
            {
                if (ParentFolder == null)
                {
                    return "";
                }
                return Path.Combine(ParentFolder.FolderPath,Name);
            } 
        }
        public List<string> Files { get; set; } = new List<string>();
        public List<Folder> Folders { get; set; } = new List<Folder>();

        public Folder ParentFolder { get; set; }

        public Folder GetFolderByLevel(int level)
        {
            if (level == 0)
            {
                return this;
            }
            else
            {
                var folders = new List<Folder>(Folders);
                folders.Reverse();
                foreach (Folder folder in folders)
                {
                    Folder result = folder.GetFolderByLevel(level - 1);
                    if (result!= null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}

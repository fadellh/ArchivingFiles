using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArchivingFiles
{
    public interface IManageFolder
    {
        //public List<string> defaults { get; set; }
        List<string> ScanDefaultFolder(string currentFolder, int inputLayer, int layer);
        int SplitDir(string path);
        void DeleteFiles(string filePath, int time);
        void DeleteFolders(string filePath, int time, int layer);
    }
}
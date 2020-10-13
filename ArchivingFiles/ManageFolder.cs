using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArchivingFiles
{
    public class ManageFolder
    {
        public List<string> defaults { get; set; }
        public ManageFolder()
        {
            this.defaults = new List<string>();
        }
        //  List<string> defaults = new List<string>(new string[] { });

        public List<string> ViewDefaultFolder(string currentFolder, int layer)
        {
            var dirName = "";
            string dateFormat = "yyyyMMdd";
            string strBaseDirPath = currentFolder;
            var existDir = Directory.GetDirectories(strBaseDirPath);
            DirectoryInfo baseDir = new DirectoryInfo(strBaseDirPath);
            int inputLayer = layer;
            inputLayer--;

            if (existDir.Length > 0)
            {
                DirectoryInfo[] subDirectories = baseDir.GetDirectories();

                foreach (var folder in subDirectories)
                {
                    DateTime dtName;
                    dirName = folder.Name.Trim();
                    // Console.WriteLine(dirName);
                    Console.WriteLine(folder.FullName);
                    defaults.Add(folder.FullName);

                    if (inputLayer > 0)
                    {
                        ViewDefaultFolder(folder.FullName, inputLayer);
                    }
                }
            }
            var result = defaults;
            return result;
        }

        public bool EraseDirectory(string folderPath, List<string> defaultFolder)
        {
           // bool recursive = false;
            //Safety check for directory existence.

            if (!Directory.Exists(folderPath))
                return false;

            foreach (string file in Directory.GetFiles(folderPath))
            {
                Console.WriteLine("Delete file {0}", file);
                // File.Delete(file);
            }

            //Console.WriteLine(recursive);
            if (FilterCollection(folderPath, defaultFolder))
            {
                Console.WriteLine("MASUK RECURSIVE");
                // recursive = true;
                // if (recursive)
                {
                    foreach (string dir in Directory.GetDirectories(folderPath))
                    {
                        EraseDirectory(dir, defaultFolder);
                    }
                }
                Console.WriteLine("Delete Directory {0}", folderPath);
                //Directory.Delete(folderPath);
            }


            //if input == string list folder yang jangan di delete return recursive false --Jadi 
            // Dia ga akan recursive lagi
            //Iterate to sub directory only if required.
            //Delete the parent directory before leaving
            return true;
        }

        public static bool FilterCollection(string path, List<string> collection)
        {
            bool same = false;
            foreach (var item in collection)
            {

                if (path == item)
                {
                 Console.WriteLine("{0} SAMA DENGAN {1}", path, path);
                    same = true;
                }
            }
            // return true;
            if (same)
            {
                return false;
            }
            else 
            {
                Console.WriteLine("{0} TIDAK SAMA Item di Folder Collection", path);
                return true;
            }
            // return false;
        }

    }
}

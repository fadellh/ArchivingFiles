using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArchivingFiles
{
    public class ManageFolder : IManageFolder
    {
        private List<string> defaults { get; set; }
        private readonly ILogger<ManageFolder> _logger;
        private readonly AppSetting _appSetting;

        public ManageFolder( ILogger<ManageFolder> logger, IOptions<AppSetting> appSetting)
        {
            _logger = logger;
            defaults = new List<string>();
            _appSetting = appSetting.Value;

        }

        public List<string> ScanDefaultFolder(string currentFolder, int inputLayer, int layer)
        {
            /*
             
            Will be scanning not deleted folder based on folder layer from path C:\TestAja\ . 
            Example string currentFolder: C:\TestAja\ have 2 layer. If inputLayer: 2 (no output) . 
            C:\TestAja\Output have 3 layer. If inputLayer 3 -> Ouput All folderPath in C:\TestAja\
            
            */
            _logger.LogInformation(_appSetting.UnitName);

            var existDir = Directory.GetDirectories(currentFolder);
            DirectoryInfo baseDir = new DirectoryInfo(currentFolder);

            if (existDir.Length > 0)
            {
                DirectoryInfo[] subDirectories = baseDir.GetDirectories();

                foreach (var folder in subDirectories)
                {
                    if (inputLayer > layer)
                    {
                       _logger.LogInformation(folder.FullName);
                        // Console.WriteLine(folder.FullName);
                        defaults.Add(folder.FullName);
                        ScanDefaultFolder(folder.FullName, inputLayer - 1, layer);
                    }
                }
            }
            var result = defaults;
            return result;
        }
        public int SplitDir(string path)
        {
            string[] splitDir = path.Split("\\");
            return splitDir.Length;
        }
        public void DeleteFiles(string filePath, int time)
        {
            string[] files = Directory.GetFiles(filePath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);

                Console.WriteLine(DateTime.Now.AddDays(-time));

                if (fi.LastAccessTime < DateTime.Now.AddDays(-time))
                {
                    _logger.LogInformation("Delete file {0}", fi.FullName);
                   // Console.WriteLine("Delete file {0}", fi.FullName);
                     //fi.Delete();
                }
            }
        }
        public void DeleteFolders (string filePath, int time, int layer)
        {
            var dirName = "";
            string[] splitDir = filePath.Split("\\");

            foreach (var list in splitDir)
            {
                // Console.WriteLine(list);
                //expected output [C:,TestAja,Output,AdmissionNo]
            }
            //if dir in the 4th layer. Delete all file and folder in that directory(example: C:\TestAja\Output\AdmissionNo)
            if (splitDir.Length > layer-1)
            {
                DirectoryInfo baseDir = new DirectoryInfo(filePath);
                DirectoryInfo[] subDirectories = baseDir.GetDirectories();

                foreach (var folder in subDirectories)
                {
                    dirName = folder.Name.Trim();
                    if (folder.CreationTime < DateTime.Now.AddDays(-time))
                    {
                        _logger.LogInformation("Delete Folder {0}", dirName);
                      //  Console.WriteLine("Delete Folder {0}", dirName);
                    // folder.Delete(true);
                    }

                }
            }

        }
    }
}

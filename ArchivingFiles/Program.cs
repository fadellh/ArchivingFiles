using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ArchivingFiles
{

    class Program
    {
        static void Main(string[] args)
        {
            var dir =@"C:\TestAja\" ;
            // Classs Delete Folder yang memiliki subfolder
            // Buat class yang berguna untuk delete folder yang memiliki file

            var manageFolder = new ManageFolder();
            var defaultFolder = manageFolder.ViewDefaultFolder(dir, 2);

            Console.WriteLine("------------------------");
            //defaultFolder.ForEach(i => Console.Write(string.Join("\t", i)));

            foreach (var item in defaultFolder)
            {   //Class untuk delete folder yang memiliki file
                string[] files = Directory.GetFiles(item);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    //    Console.WriteLine(fi.FullName);
                    if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
                        Console.WriteLine(fi);
                       // fi.Delete();
                }

                //Split folder(item) yang memiliki 3 \ (garis/path) untuk di delete subfoldernya

                var dirName = "";
                string dateFormat = "yyyyMMdd";
                string[] tokens = item.Split("\\");
                    // Console.WriteLine("-----HASIL SPLIT---------");
                foreach (var list in tokens)
                {
                     Console.WriteLine(list);
                }
                   //  Console.WriteLine("-----HASIL SPLIT---------");
                if (tokens.Length > 3) 
                {
                    DirectoryInfo baseDir = new DirectoryInfo(item);
                    DirectoryInfo[] subDirectories = baseDir.GetDirectories();

                    foreach (var folder in subDirectories)
                    {
                        DateTime dtName;
                        dirName = folder.Name.Trim();
                       // Console.WriteLine(dirName);
                        //Console.WriteLine(folder.FullName);
                          Console.WriteLine("Delete Folder {0}", dirName);
                        if (DateTime.TryParseExact(dirName, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtName))
                        {
                            if (DateTime.Today.Subtract(dtName).TotalDays > 30)
                                folder.Delete(true); //to delete all the files and subdirectory inside this directory, recursively
                        }
                    }
                }
                //class untuk delete folder yang memiliki folder
               /* DirectoryInfo d = new DirectoryInfo(item);
                if (d.CreationTime < DateTime.Now.AddDays(-7))
                        Console.WriteLine(d);
                    //d.Delete();*/

            }



            //Console.WriteLine(folder);














            /*
                        if (subDirectories != null && subDirectories.Length > 0)
                        {
                            DateTime dtName;
                            for (int j = subDirectories.Length - 1; j >= 0; j--)
                            {
                                Console.WriteLine("gai");
                                dirName = subDirectories[j].Name.Trim();
                                Console.WriteLine(dirName);
                                if (DateTime.TryParseExact(dirName, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtName))
                                {
                                    if (DateTime.Today.Subtract(dtName).TotalDays > 30)
                                        // subDirectories[j].Delete(true); //to delete all the files and subdirectory inside this directory, recursively
                                        Console.WriteLine(dirName);
                                }
                            }
                        }*/

            /*        DirectoryInfo d = new DirectoryInfo(dir);
                    if (d.CreationTime < DateTime.Now.AddDays(-7))
                        Console.WriteLine("");
        */
        }
    }
}

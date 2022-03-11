using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using SharpCompress;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Archives.GZip;
using System.IO;
using SharpCompress.Compressors.Deflate;
using System.Linq;
using SharpCompress.Writers;
using SharpCompress.Readers;
using SharpCompress.Archives.Tar;

namespace ArchivingFiles
{
    public class Executor : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<Executor> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly AppSetting _appSetting;



        // private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public Executor(ILogger<Executor> logger,
            IServiceProvider services, IHostApplicationLifetime hostApplicationLifetime, IOptions<AppSetting> appSetting
            )
        {
            _logger = logger;
            _services = services;
            _appSetting = appSetting.Value;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {



                using (var scope = _services.CreateScope())
                {
                    var stopwatch = Stopwatch.StartNew();
                    var manageFolder = scope.ServiceProvider.GetRequiredService<IManageFolder>();
                    try
                    {
                        // var manageFolder = new ManageFolder();

                        ///----------------------
                        //App Setting.json

                        // var dir = _appSetting.UnitName;

                        string TEST_ARCHIVES_PATH = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Test";
                        string TEMP = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Temp";
                        string SCRATCH_FILES_PATH = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Res\\Testpd.tar";
                        string RES = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Res";
                        string TAR_PATH = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Testpd.tar.gz";

                        /*                        //string jpg = Path.Combine(ORIGINAL_FILES_PATH, "jpg", "test.jpg");
                                                using (Stream stream = File.OpenRead(TEST_ARCHIVES_PATH))
                                                using (var archive = GZipArchive.Open(stream))
                                                {
                                                    _logger.LogInformation("Begin");
                                                    archive.SaveTo(targetName);
                                                    _logger.LogInformation("End");
                                                }*/

                        /*                        using (Stream stream = File.OpenRead(Path.Combine(TEST_ARCHIVES_PATH, "Tar.tar.gz")))
                                                using (var archive = GZipArchive.Open(stream))
                                                {
                                                    _logger.LogInformation("Begin");
                                                    var entry = archive.Entries.First();
                                                    entry.WriteToFile(Path.Combine(SCRATCH_FILES_PATH, entry.Key));

                                                    long size = entry.Size;
                                                    var scratch = new FileInfo(Path.Combine(SCRATCH_FILES_PATH, "Tar.tar"));
                                                    var test = new FileInfo(Path.Combine(TEST_ARCHIVES_PATH, "Tar.tar"));
                                                    _logger.LogInformation("Finish");



                                                }*/


                        /*                        using (Stream stream = File.OpenWrite(TAR_PATH))
                                                using (var writer = WriterFactory.Open(stream, ArchiveType.Tar, new WriterOptions(CompressionType.GZip)
                                                {

                                                }))
                                                {
                                                    writer.WriteAll(TEST_ARCHIVES_PATH, "*", SearchOption.AllDirectories);
                                                }*/


                        /*                        using (var archive = ZipArchive.Create())
                                                {
                                                    _logger.LogInformation("begin");
                                                    archive.AddAllFromDirectory(TEST_ARCHIVES_PATH);
                                                    archive.SaveTo(SCRATCH_FILES_PATH, CompressionType.Deflate);
                                                    _logger.LogInformation("finish");
                                                }*/

                        using (var archive = TarArchive.Create())
                        {
                            archive.AddAllFromDirectory(TEST_ARCHIVES_PATH);
                            archive.SaveTo(SCRATCH_FILES_PATH,CompressionType.None);
                        }

                        using (var archive = GZipArchive.Create())
                        {
                            archive.AddAllFromDirectory(RES);
                            archive.SaveTo(TAR_PATH, CompressionType.Deflate);
                        }




                        /*
                        }*/





                        /*                        string sourceName = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Test";
                                                string targetName = "C:\\Users\\bithealth-fdl\\Desktop\\Compressor\\Test.gz";

                                                // 1
                                                // Initialize process information.
                                                //
                                                ProcessStartInfo p = new ProcessStartInfo();
                                                p.FileName = "C:\\Program Files\\7-Zip\\7z.exe";

                                                // 2
                                                // Use 7-zip
                                                // specify a=archive and -tgzip=gzip
                                                // and then target file in quotes followed by source file in quotes
                                                //
                                                p.Arguments = "a -t7z \"" + targetName + "\" \"" +
                                                    sourceName + "\" -mx=9";
                                                p.WindowStyle = ProcessWindowStyle.Hidden;

                                                // 3.
                                                // Start process and wait for it to exit
                                                //
                                                Process x = Process.Start(p);
                                                x.WaitForExit();*/



                        /*                        _logger.LogInformation(_appSetting.UnitName);

                                                var dir = @"C:\Test23";
                                                var keepFolderLayer = 4;
                                                var deleteFolderDay = -60;
                                                //------------------

                                                if (dir.EndsWith("\\"))
                                                {
                                                    dir = dir.Remove(dir.Length - 1);
                                                }

                                                var layerInput = manageFolder.SplitDir(dir);

                                                if (keepFolderLayer <= layerInput)
                                                {
                                                    _logger.LogWarning("Please");
                                                }

                                                var defaultFolder = manageFolder.ScanDefaultFolder(dir, keepFolderLayer, layerInput);

                                                Console.WriteLine("------------------------");

                                                if (deleteFolderDay > 0) 
                                                {
                                                    foreach (var item in defaultFolder)
                                                    {
                                                        manageFolder.DeleteFiles(item, deleteFolderDay);
                                                        manageFolder.DeleteFolders(item, deleteFolderDay, keepFolderLayer);

                                                    }
                                                }
                                                _logger.LogInformation("Finish");*/

                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());

                    }


                    _logger.LogInformation("Time archivingFiles:{0}", stopwatch.Elapsed.TotalMilliseconds);

                    stopwatch.Stop();

                    _logger.LogInformation("Time delete document:{0}", stopwatch.Elapsed.TotalMilliseconds); //pritn to log json

                    _hostApplicationLifetime.StopApplication();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                
            }
            
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}


    
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

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
                        _logger.LogInformation(_appSetting.UnitName);

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
                        _logger.LogInformation("Finish");

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


    
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestWorkerService {

    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        readonly IConfiguration Configuration;
        List<Site> sites = new List<Site>();

        public Worker(ILogger<Worker> logger) {
            _logger = logger;
            var appsettingsjson = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            Configuration = new ConfigurationBuilder().AddJsonFile(appsettingsjson).Build();
            var sitesSection = Configuration.GetSection("Sites").GetChildren();
            foreach(var site in sitesSection) {
                var s = new Site();
                s.Tag = site.Key;
                s.Url = site.Value;
                sites.Add(s);
            }
        }

        //public override Task StartAsync(CancellationToken cancellationToken) {
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while(!stoppingToken.IsCancellationRequested) {

                foreach(var site in sites) {
                    var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    var response = await (new Aping(site.Url, site.Tag)).Ping();
                    _logger.LogInformation($"{now} APING {response}");
                }
                await Task.Delay(15*60*1000, stoppingToken);
            }
        }

        //public override Task StopAsync(CancellationToken cancellationToken) {
        //}
    }
}

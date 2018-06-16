using System;

using Common;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Redux
{
    public class Program
    {
        public static ReduxControl Control;

        public static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Logger.Initialize("Redux", baseDir, true);

            Control = new ReduxControl();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options => {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                })
                .Build();
    }
}

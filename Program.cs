using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProyectoLibros
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((env, config) =>
                {
                    // aquí colocamos la configuración de proveedores 
                    var ambiente = env.HostingEnvironment.EnvironmentName;
                    config.AddJsonFile($"appsettings.{ambiente}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

                    var currentConfig = config.Build();

                    // Estas configuraciones se deben guardar fuera del código fuente del App.
                    config.AddAzureKeyVault(currentConfig["Vault"],
                        currentConfig["ClientId"],
                       currentConfig["ClientSecret"]);
                })
                .UseStartup<Startup>();


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

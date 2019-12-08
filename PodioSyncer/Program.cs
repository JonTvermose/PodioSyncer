using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PodioSyncer.Data;

namespace PodioSyncer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                using(var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var config = services.GetRequiredService<IConfiguration>();

                    // Automagically apply all migrations
                    var dxbuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    dxbuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                    using (var context = new ApplicationDbContext(dxbuilder.Options))
                    {
                        context.Database.Migrate();
                    }

                    var dxLogbuilder = new DbContextOptionsBuilder<LogDbContext>();
                    dxLogbuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                    using (var context = new LogDbContext(dxLogbuilder.Options))
                    {
                        context.Database.Migrate();
                    }
                }
                host.Run();

            } catch (Exception e)
            {
                using (var sw = new StreamWriter(new FileStream("StartupErrors.txt", FileMode.Append, FileAccess.Write)))
                {
                    sw.WriteLine($"{DateTime.UtcNow.ToString("[yyyy-MM-dd][HH:mm:ss]")} {e}");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, config) => {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using persistence;
using Persistence;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostserver = CreateHostBuilder(args).Build();
            using(var enviroment = hostserver.Services.CreateScope()){
                var services = enviroment.ServiceProvider;

                try{
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var context = services.GetRequiredService<OntoSoftContext>();
                    context.Database.Migrate();
                    // DataPrueba.InsertData(context, userManager).Wait();
                }catch(Exception e){
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e, "Ocurrio un error en la migración");
                }
                
            }
            hostserver.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

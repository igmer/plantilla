using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversalIdentity.Data;
using UniversalIdentity.Models;

namespace UniversalIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            /*
            //Quite el bloque de comentarios para esta función al finalizar la primera migración
            //Este bloque llama a los métodos necesarios para crear el usuario y rol de Adminsitración
            //Para modificar los valores con que se creará el usuario Administrador modifique los valoes en el método DataInitializer
            //Una vez creado el usuario administrador comente o elimine este bloque.
            //--------------------------
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                    DataInitializer.SeedData(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            */
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

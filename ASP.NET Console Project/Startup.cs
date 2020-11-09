using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLContactsLibrary;
using System;
using Settings;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

namespace ASP.NET_Console_Project
{
    //Singleton class has a private constrictpr, an inversion of IOC container
    // since the class is managed by an IOC container, our singleton can now have dependencies of its own injected into it.

    //An ioc container is also used to retrieve a service Serviceprovicer.getservice<service>();
    public class Startup
    {
        public static ContactView View { get { return ServiceProvider.GetService<ContactView>(); } }

        //static variables are created on FIRST call to the class static method or property
        //the singleton object can be obtained by accessing the Instance property
        private static readonly Startup _startup = new Startup();
        private IConfiguration Configuration { get; set; }

        //service provider property must be static because the static view property needs to access it
        private static ServiceProvider ServiceProvider { get; set; }


        private Startup()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            // this code is not neccesary in an MVC project
            //same as creating instance and then calling each method one at a time
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


            var config = builder.Build();
            Configuration = (IConfiguration)config;

            //Long way, short way
            //string currentConnectionString = Configuration.GetConnectionString("Default");
            //string currentConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("Default").Value;
            //create instances as services

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<ContactView>();
            services.AddSingleton<IContacts, SQLContacts>();


            // How to inject a string parameter. Must build the Serive Provider first!
            // Build the Service Provider now to create IAppSettings Service
           // ServiceProvider = services.BuildServiceProvider();
            //Build the service providewr now to creat iappsettings
            //mnaually create the singleton yourself, plus the string
            //  services.AddSingleton<IDataAccess>(provider =>
            //  new SQLDataAccess(ServiceProvider.GetService<IAppSettings>(),
            // ServiceProvider.GetService<IAppSettings>().CurrentConnectionString));
            //reBuild the sp after all services are created!! WHY MICROSOFT WHY
            services.AddSingleton<IDataAccess, SQLDataAccess>();

            ServiceProvider = services.BuildServiceProvider();



        }
    }
}

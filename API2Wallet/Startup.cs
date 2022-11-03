using API2Wallet.Class;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace API2Wallet
{
    public class Startup
    {
        string tC_AppName;
        string tC_AppVer;
        string tC_RunTimeVer;
        public static string tC_VirtualPath;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cAppSettings.tConnectionString =  configuration.GetConnectionString("API2WalletConnection");

            tC_VirtualPath = Environment.GetEnvironmentVariable("ENV_VirtualPath");
            tC_AppName = Assembly.GetExecutingAssembly().GetName().Name;
            tC_AppVer = Assembly.GetEntryAssembly().GetName().Version.ToString();
            tC_RunTimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{tC_AppName} V{tC_AppVer}", Version = $"{tC_RunTimeVer}" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{tC_AppName} V{tC_AppVer}"));
            //}

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "HomePage")),
            //    RequestPath = "",
            //    EnableDefaultFiles = true
            //});

            //*Ton 2021-08-25
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{tC_VirtualPath}/swagger/v1/swagger.json", $"{tC_AppName} V{tC_AppVer}");
            });
        }
    }
}

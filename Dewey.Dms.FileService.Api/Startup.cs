using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Repository;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Hdfs.Client;
using Dewey.Dms.FileService.Hdfs.Client.Web;
using Dewey.Dms.FileService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dewey.Dms.FileService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
           
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
             });
            
            IEnumerable<IConfigurationSection> hbaseSection = Configuration.GetSection("HBase").GetChildren();
            IEnumerable<IConfigurationSection> hdfsSection = Configuration.GetSection("Hdfs").GetChildren();
           
            string hbaseRestAddress =
                hbaseSection.Where(a => a.Key == "RestAddress").Select(a => a.Value).SingleOrDefault();

            string webHdfsAddress = hdfsSection.Where(a => a.Key == "WebHdfsAddress").Select(a=>a.Value).SingleOrDefault();
            
            if (!string.IsNullOrEmpty(hbaseRestAddress) && !string.IsNullOrEmpty(webHdfsAddress))
            {
                services.AddSingleton<IDatabaseService>(new RestDatabaseService(hbaseRestAddress));

                services.AddSingleton<IHdfsClient>(new WebHdfsClient(webHdfsAddress, true));
                services.AddSingleton<IDmsFileOperations, DmsFileOperations>();
                services.AddSingleton<IFileUserRepository, DmsFileUserRepository>();
            }
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UploadMiddleware.AliyunOSS;
using UploadMiddleware.Core;
using UploadMiddleware.LocalStorage;

namespace Example
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
            services.AddControllersWithViews(options => { options.Filters.Add<GlobalFilter>(); });
            services.AddUploadLocalStorage(options =>
            {
                options.RootDirectory = Configuration.GetSection("SaveRootDirectory").Value;
                options.AllowFileExtension.Add(".mp4");
                //options.AddUploadCompletedHandler<CustomUploadCompletedHandler>();
            });

            //services.AddChunkedUploadLocalStorage(options =>
            //{
            //    options.AllowFileExtension.Add(".zip");
            //    options.AllowFileExtension.Add(".mp4");
            //    options.RootDirectory = Configuration.GetSection("SaveRootDirectory").Value;
            //    options.DeleteChunksOnMerged = true;
            //    options.ChunksRootDirectory = Path.Combine(Configuration.GetSection("SaveRootDirectory").Value, "��Ƭ");
            //});

            //services.AddUploadAliyunOSS(options =>
            //{
            //    options.AccessId = Configuration.GetSection("OSS:AccessId").Value;
            //    options.AccessKeySecret = Configuration.GetSection("OSS:AccessKeySecret").Value;
            //    options.BucketName = Configuration.GetSection("OSS:BucketName").Value;
            //    options.Endpoint = Configuration.GetSection("OSS:Endpoint").Value;
            //    options.RootDirectory = Configuration.GetSection("OSS:RootDirectory").Value;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            //app.UsePathBase(Directory.GetCurrentDirectory());
            app.UseUpload("/upload");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

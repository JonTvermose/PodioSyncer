using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Mappings;
using PodioSyncer.Options;
using PodioSyncer.Services;

namespace PodioSyncer
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
            // Database
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            // Authentication
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                    .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddControllersWithViews();

            // Automapper
            services.AddAutoMapper(new Type[] { typeof(MappingProfile) });

            // Dependency injection
            services.Configure<ConfigurationOptions>(Configuration);
            services.AddTransient<ConfigurationOptions>();
            services.AddTransient<QueryDb>();
            services.AddTransient<SyncService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Commands
            services.AddTransient<VerifyWebhookCommand>();
            services.AddTransient<CreatePodioApp>();
            services.AddTransient<UpdatePodioApp>();
            services.AddTransient<DeletePodioApp>();
            services.AddTransient<CreateLink>();
            services.AddTransient<UpdateLink>();
            services.AddTransient<CreateSyncEvent>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapFallbackToController("Index", "Home");
            });

        }
    }
}

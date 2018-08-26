using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using CapStore.Models;   // Entity of module names
using CapStore.Repositories;
using CapStore.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; //Built-in autentification 
using Microsoft.AspNetCore.Http;

namespace CapStore
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
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CapContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<Customer, IdentityRole>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<CapContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ICapRepository, CapRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => Cart.GetCart(sp));
           

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "capdetails",
                   template: "Cap/Details/{capId?}",
                   defaults: new { Controller = "Cap", action = "Details" });

                routes.MapRoute(
                    name: "categoryfilter",
                    template: "Cap/{action}/{category?}",
                    defaults: new { Controller = "Cap", action = "List" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

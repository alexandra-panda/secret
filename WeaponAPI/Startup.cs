using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WeaponAPI.DbContext;
using WeaponAPI.Repositories;
using WeaponAPI.Repositories.Abstracts;
using WeaponAPI.Services;
using WeaponAPI.Services.Abstracts;
using WeaponAPI.Settings;
using WeaponAPI.Settings.Abstracts;

namespace WeaponAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WeaponDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:WeaponDb"]));

            services.AddHttpContextAccessor();
            services.Configure<SyncServiceSettings>(Configuration.GetSection("SyncServiceSettings"));
            
            services.AddSingleton<ISyncServiceSettings>(provider =>
                provider.GetRequiredService<IOptions<SyncServiceSettings>>().Value);
            
            services.AddControllers();
            
            services.AddScoped<IWeaponRepository, WeaponRepository>();
            services.AddScoped<ISyncService<Weapon>, SyncService<Weapon>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
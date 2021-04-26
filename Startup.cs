using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TG.Entity;
using TG.Interface;
using TG.Models;
using TG.Models.Settings;
using TG.Repository;

namespace TG
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public  IConfiguration Configuration { get; }
        public  void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(connection);
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddSession();
            services.AddTransient<IControllerService, ControllerService>();
            services.AddSingleton<IUserRepository, UserRepository>();          
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Data", Version = "v1" });              
            });
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");


            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();        

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            Bot.GetBotClient();
        }
    }
}

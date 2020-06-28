using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using identity.Data.Abstraction;
using identity.Data.Extension;
using identity.Data.Model;
using identity.Data.Repository;
using IdentityServer.configuration;
using IdentityServer.Mapping;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var builder = services.AddIdentityServer()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<ResourceOwnerPasswordValidator>>()
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<User>();

            builder.AddDeveloperSigningCredential();
            services.AddHttpClient();
            services.AddDbContext<IdentityContext>(opt =>
                {
                    opt.UseSqlServer(Configuration["IdentitySever:ConnectionString"]);
                });
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;


            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders(); 
            services.AddScoped<IUser,UserRepository>();
            services.AddAutoMapper(typeof(CreateProfile));
            services.AddAuthorization();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}

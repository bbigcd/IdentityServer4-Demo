﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServerApiMerge {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
            services.AddIdentityServer (options => options.Authentication.CookieAuthenticationScheme = "Cookies")
                .AddDeveloperSigningCredential ()
                .AddInMemoryIdentityResources (Config.GetIdentityResources ())
                .AddInMemoryApiResources (Config.GetApis ())
                .AddInMemoryClients (Config.GetClients ())
                .AddTestUsers (Config.GetUsers ()); //添加测试用户

            services.AddAuthentication ("Bearer")
                .AddCookie ("Cookies")
                .AddJwtBearer ("Bearer", options => {
                    options.Authority = "http://localhost:6000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseIdentityServer ();

            app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}
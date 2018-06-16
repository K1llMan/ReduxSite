﻿using System;
using System.Text;

using Common;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Redux
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
            // Аутентификация пользователя
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })
                .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.Control.Settings.JWTKey)),

                        // Любой Issuer и любой Audience
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ValidateLifetime = true, //validate the expiration and not before values in the token

                        ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                    };
                });

            services.AddMvc();
        }

        /// <summary>
        /// Разрешение отдавать файлы указанных типов
        /// </summary>
        private StaticFileOptions GetStaticFileConfiguration()
        {
            var provider = new FileExtensionContentTypeProvider { Mappings = { [".tmp"] = "text/html" } };
            return new StaticFileOptions { ContentTypeProvider = provider };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopped.Register(OnStopped);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Включение аутентификации
            app.UseAuthentication();

            // Статический контент
            app.UseDefaultFiles();
            app.UseStaticFiles(GetStaticFileConfiguration());

            app.UseMvc();
        }

        #region Lifetime events

        private void OnStopped()
        {
            // Корректное уничтожение объекта
            Program.Control?.Dispose();
            Logger.CloseLogFile();
        }

        #endregion Lifetime events
    }
}

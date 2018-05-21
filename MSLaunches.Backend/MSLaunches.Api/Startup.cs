﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSLunches.Api.Authorization;
using MSLunches.Api.Filters;
using MSLunches.Api.Middleware;
using MSLunches.Data.EF;
using MSLunches.Domain.Services;
using MSLunches.Domain.Services.Interfaces;
using MSLunches.Infrastructure.AuthZero;
using MSLunches.Infrastructure.RestClient;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace MSLunches.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebApiCoreLunchesContext>(options => options.UseInMemoryDatabase(Environment.MachineName));

            // Add framework services.
            services.AddMvc();

            IAuthorizationPolicies authorizationPolicies = new AuthorizationPolicies();
            services.AddSingleton(authorizationPolicies);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(AuthorizationPolicies.AdminOnly), authorizationPolicies.AdminOnly);
            });

            //Registers the use of a jwt token
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.Audience = Configuration["auth0:clientId"];
                option.Authority = $"https://{Configuration["auth0:domain"]}/";
            });

            //Creates the swagger json based on the documented xml/attributes of the endpoints
            services.AddSwaggerGen(c =>
            {
                //Metadata of the api
                c.SwaggerDoc("v1", GetSwaggerDoc());
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<ValidateModelResponseOperationFilter>();
            });

            // Register Infrastructure dependencies
            services.AddScoped<IRestClient>(sp => new RestClient($"https://{Configuration["auth0:domain"]}", new HttpClient()));
            services.AddSingleton<IAuthZeroClient>(sp => new AuthZeroClient(sp.GetRequiredService<IRestClient>(), Configuration["auth0:NonInteractiveClientId"], Configuration["auth0:NonInteractiveClientSecret"], Configuration["auth0:domain"]));
            services.AddTransient<IAuthZeroService>(sp => new AuthZeroService(sp.GetRequiredService<IAuthZeroClient>()));

            // Register Services
            services.AddTransient<IUserService>(sp => new UserService(sp.GetRequiredService<WebApiCoreLunchesContext>()));
            services.AddTransient<ILunchService>(sp => new LunchService(sp.GetRequiredService<WebApiCoreLunchesContext>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WebApiCoreLunchesContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware(typeof(AuthorizationMiddleware));

            //Enable swagger midleware
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //Sets swagger UI route on root, "GET {baseUrl}/ "
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiCoreMSLunches V1");
            });
            
            app.UseMvc();
            DatabaseMSLunches.Initialize(dbContext);
        }

        /// <summary>
        /// Returns swagger metadata
        /// </summary>
        /// <returns></returns>
        private static Info GetSwaggerDoc()
        {
            return new Info 
            { 
                Title = "WebApiCoreMSLunches", 
                Version = "v1",
                Description = "Web Api MSLunches for MS",
                TermsOfService = "https://github.com/MakingSense/WebApiCore-MSLunches",
                Contact = new Contact
                {
                    Name = "Gastón Cerioni",
                    Email = "gcerioni@makingsense.com"
                },
                License = new License
                {
                    Name = "I would love to put one c:",
                    Url = "https://github.com/MakingSense/WebApiCore-MSLunches"
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Sindikat.Ankete.Infrastructure;
using Sindikat.Ankete.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Sindikat.Ankete.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SOME_RANDOM_KEY_DO_NOT_SHARE"));
            var tokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                ValidateIssuer = true,
                ValidIssuer = "http://localhost",
                ValidateAudience = true,
                ValidAudience = "http://localhost",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddDbContext<Persistence.AnketeDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AnketeDatabase"));
            });

            

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            //.AddJwtBearer(configureOptions =>
            // {

            //     configureOptions.RequireHttpsMetadata = false;
            //     configureOptions.ClaimsIssuer = "http://localhost";
            //     configureOptions.TokenValidationParameters = tokenValidationParameters;
            //     configureOptions.SaveToken = true;
            // });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(Options => 
            Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Sindikat.Identity API", Version = "v1"
                });
            var security = new Dictionary<string, IEnumerable<string>>
            {
                {
                    "Bearer", new string[]
                    {
                    }
                },
            };
                c.AddSecurityDefinition("Bearer", 
            new ApiKeyScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization", In = "header", Type = "apiKey"
            });
                c.AddSecurityRequirement(security);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("StvoriAnketu", policy => policy.RequireClaim("CreateSurvey"));
                options.AddPolicy("IspuniAnketu", policy => policy.RequireClaim("TakeSurvey"));
                options.AddPolicy("Rezultati", policy => policy.RequireClaim("ViewSurveyResults"));
            });

            builder.Populate(services);
            builder.RegisterModule(new InfrastructureModule());
            builder.RegisterModule(new PersistenceModule());

            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sindikat.Identity API");
            c.DocumentTitle = "Title Documentation";
            c.DocExpansion(DocExpansion.None); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

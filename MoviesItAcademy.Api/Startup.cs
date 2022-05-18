using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoviesItAcademy.Api.Infrastructure.Extensions;
using MoviesItAcademy.Api.Infrastructure.Mappings;
using MoviesItAcademy.Api.Infrastructure.Middlewares;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.PersistanceDb.Context;
using MoviesItAcademy.Service.Models.Jwt;
using System;
using MoviesItAcademy.Api.Infrastructure.Healthchecks;
using MoviesItAcademy.PersistanceDb.Seed;
using MoviesItAcademy.Api.Controllers.v1;

namespace MoviesItAcademy.Api
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
            services.AddControllers().
                AddFluentValidation(Configuration => Configuration.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddServicesAndRepositories();
            services.MappingRegistration();

            services.AddTokenAuthentication(Configuration);
            services.Configure<JwtConfiguration>(Configuration.GetSection(nameof(JwtConfiguration)));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            })
                    .AddEntityFrameworkStores<MoviesItAcademyContext>();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddDbContext<MoviesItAcademyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MoviesItAcademy",
                    Version = "v1",
                    Description = "MoviesItAcademy API designed to serve user/guest actions.",
                    Contact = new OpenApiContact
                    {
                        Name = "Healthchecks UI",
                        Url = new Uri("https://localhost:44357/healthchecks-ui")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into the given field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                    new string[] { }
                    }
                });
            });

            services.AddHealthChecks()
                .AddDbContextCheck<MoviesItAcademyContext>(nameof(MoviesItAcademyContext))
                .AddCheck<AccountHealthChecker>(nameof(AccountController))
                .AddCheck<BookingHealthChecker>(nameof(BookingController))
                .AddCheck<MovieHealthChecker>(nameof(MovieController));

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(10);
                options.AddHealthCheckEndpoint("API Health Check", "/health");
            }).AddInMemoryStorage();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesItAcademy API v1");
            });

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlerApiMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI();

                endpoints.MapControllers();
            });

            MoviesItAcademySeed.Initialize(app.ApplicationServices);
        }
    }
}

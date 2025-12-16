using Application;
using FluentValidation;
using Gridify;
using Infrastructure;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.ViewModels;
using WebApi.ViewModels.ViewModelsValidator;
using WebApi.Workers;

namespace WebApi
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
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.AddHostedService<ConsultaMonitoramentoWorker>();
            services.AddHostedService<LimpezaDadosAntigosWorker>();
            services.AddScoped<IValidator<AutenticacaoViewModel>, AutenticacaoViewModelValidator>();
            services.AddHttpClient();


            services.AddSwaggerGen(c =>
            {
                var desc = $"API Clinica Integrada <br />{new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime}";
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebApi",
                    Description = desc
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "identity",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Configuration["TokenConfiguration:Audience"],
                    ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseHttpsRedirection();
            SeedDatabaseAsync(app);
            app.UseRouting();
            app.UseCors("AllowAnyOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static async void SeedDatabaseAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var geracaoUsuariosPerfisIniciais = serviceScope.ServiceProvider.GetService<IDbContextSeed>();

            geracaoUsuariosPerfisIniciais.GerarPerfis();
            //geracaoUsuariosPerfisIniciais.GerarUsuarios();
            //await geracaoUsuariosPerfisIniciais.GerarProfissionaisEEquipesAsync();
            //await geracaoUsuariosPerfisIniciais.GerarSalasAsync();
        }
    }
}


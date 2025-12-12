using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Identity.Services;
using Infrastructure.Identity.Services.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
            });

            services.AddIdentity<Usuario, Perfil>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts => {
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = false;
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IDbContextSeed, DbContextSeed>();
            services.AddScoped<IDateTime, DateTimeService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUsuarioLogado, UsuarioLogado>();

            return services;
        }

    }
}

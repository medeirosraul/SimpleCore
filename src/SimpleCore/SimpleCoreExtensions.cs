using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleCore.Base.Services;
using SimpleCore.Contexts;
using SimpleCore.Data;
using SimpleCore.Identities.Entities;
using SimpleCore.Identities.Services;

namespace SimpleCore
{
    public static class SimpleCoreExtensions
    {
        public static IServiceCollection AddSimpleCore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            return services;
        }

        public static IServiceCollection AddSimpleCoreIdentity<TIdentity>(this IServiceCollection services)
            where TIdentity : Identity, new()
        {
            services.AddScoped<IIdentityService<TIdentity>, IdentityService<TIdentity>>();
            services.AddScoped<IIdentityProvidedService, IdentityProvidedService>();
            services.AddScoped<IIdentityContext<TIdentity>, IdentityContext<TIdentity>>();

            return services;
        }

        /// <summary>
        /// Add services for SimpleCore Identity.
        /// </summary>
        /// <typeparam name="TIdentity">Identity type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TContext">DbContext Type.</typeparam>
        public static IServiceCollection AddSimpleCoreIdentity<TIdentity, TContext>(this IServiceCollection services)
            where TIdentity : Identity, new()
            where TContext : SimpleDbContext
        {
            services.AddScoped<IIdentityContext<TIdentity>, IdentityContext<TIdentity>>();

            services.AddScoped<IIdentityProvidedService, IdentityProvidedService>(s =>
            {
                return new IdentityProvidedService(s.GetRequiredService<TContext>());
            });

            services.AddScoped<IIdentityService<TIdentity>, IdentityService<TIdentity>>(s =>
            {
                return new IdentityService<TIdentity>(s.GetRequiredService<TContext>(), s.GetRequiredService<ILogger<IdentityService<TIdentity>>>(), s.GetRequiredService<IIdentityProvidedService>());
            });

            return services;
        }
    }
}
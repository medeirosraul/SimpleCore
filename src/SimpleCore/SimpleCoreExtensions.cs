using Microsoft.Extensions.DependencyInjection;
using SimpleCore.Abstractions;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Contexts;
using SimpleCore.Data;
using SimpleCore.Data.Options;
using SimpleCore.Identity;
using SimpleCore.Services;
using SimpleCore.Services.Identity;

namespace SimpleCore
{
    public static class SimpleCoreExtensions
    {
        public static IServiceCollection AddSimpleCore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<,>), typeof(Service<,>));

            return services;
        }

        public static IServiceCollection AddSimpleCoreIdentity<TIdentity, TKey>(this IServiceCollection services)
            where TIdentity : Identity<TKey>, new()
        {
            services.AddScoped<IIdentityService<TIdentity, TKey>, IdentityService<TIdentity, TKey>>();
            services.AddScoped<IIdentityContext<TIdentity, TKey>, IdentityContext<TIdentity, TKey>>();

            return services;
        }

        public static IServiceCollection AddSimpleMongo<TKey>(this IServiceCollection services, Action<SimpleMongoOptions<TKey>> options)
        {
            services.Configure(options);
            services.AddSingleton<SimpleMongoContext<TKey>>();

            return services;
        }
    }
}
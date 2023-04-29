using Microsoft.Extensions.DependencyInjection;
using SimpleCore.Abstractions;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Data;
using SimpleCore.Data.Options;
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

        public static IServiceCollection AddSimpleCoreIdentity<TKey>(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISimpleIdentityService<>), typeof(SimpleIdentityService<>));
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
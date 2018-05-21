using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IdentityServer.External.TokenExchange;
using IdentityServer.External.TokenExchange.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer.External.TokenExchange.Providers;
using IdentityServer.External.TokenExchange.Processors;
using IdentityServer.External.TokenExchange.Services;
using IdentityServer4.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extensions
    {
        public static IIdentityServerBuilder AddTokenExchangeForExternalProviders(this IIdentityServerBuilder services)
        {
            services.AddExtensionGrantValidator<ExternalAuthenticationGrant>();
            services.Services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor>();
            services.Services.AddScoped<IEmailUserProcessor, EmailUserProcessor>();
            return services;
        }

        public static IIdentityServerBuilder AddDefaultTokenExchangeProviderStore(this IIdentityServerBuilder services)
        {

            services.Services.AddScoped<ITokenExchangeProviderStore>(s =>
                new DefaultTokenExchangeProviderStore(Providers.GetProviders()));
           return services;
        }

        public static IIdentityServerBuilder AddCustomTokenExchangeProviderStore<TStore>(this IIdentityServerBuilder services) where TStore : ITokenExchangeProviderStore
        {
            services.Services.AddScoped(typeof(ITokenExchangeProviderStore), typeof(TStore));
            return services;
        }
        public static IIdentityServerBuilder AddDefaultExternalUserStore(this IIdentityServerBuilder services)
        {
            services.Services.AddScoped<IExternalUserStore, DefaultExternalUserStore>();
            return services;
        }

        public static IIdentityServerBuilder AddCustomExternalUserStore<T>(this IIdentityServerBuilder services) where T:IExternalUserStore
        {
            services.Services.AddScoped(typeof(IExternalUserStore), typeof(T));
            return services;
        }

        public static IIdentityServerBuilder AddDefaultTokenExchangeProfileService(this IIdentityServerBuilder services)
        {
            services.AddProfileService<TokenExchangeProfileService>();
            return services;
        }

        public static IIdentityServerBuilder AddCustomTokenExchangeProfileService<T>(this IIdentityServerBuilder services) where  T: IProfileService
        {
            services.Services.AddScoped(typeof(IProfileService), typeof(T));
            return services;
        }
        public static IIdentityServerBuilder AddDefaultExternalTokenProviders(this IIdentityServerBuilder services)
        {
            services.Services.AddScoped<FacebookAuthProvider>();
            services.Services.AddScoped<TwitterAuthProvider>();
            services.Services.AddScoped<LinkedInAuthProvider>();
            services.Services.AddScoped<GoogleAuthProvider>();


            
            services.Services.AddScoped<Func<string, IExternalTokenProvider>>(serviceProvider => key =>
            {                
                var name = $"{key}AuthProvider";
                var assembly = typeof(FacebookAuthProvider).Assembly;
                var type = assembly.ExportedTypes.First(x => String.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
                return (IExternalTokenProvider) serviceProvider.GetService(type);
            });

            return services;
        }

        public static IIdentityServerBuilder AddCustomExternalTokenProvider<T>(this IIdentityServerBuilder services,string providerName) where T : IExternalTokenProvider
        {

            services.Services.AddScoped(typeof(T));
            return services;
        }

        
    }
}

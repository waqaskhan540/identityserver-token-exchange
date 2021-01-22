using IdentityServer.External.TokenExchange.Models;
using System;

namespace IdentityServer.External.TokenExchange.Helpers
{
    public static class ExternalTokenExchangeProviderHelper
    {
        public static void CheckNotNull(ExternalTokenExchangeProvider provider)
        {
            if (provider == null)
            {
                var providerName = nameof(provider);
                throw new ArgumentNullException(providerName);
            }
        }
    }
}

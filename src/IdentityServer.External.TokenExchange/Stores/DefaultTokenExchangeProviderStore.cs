using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer.External.TokenExchange.Models;

namespace IdentityServer.External.TokenExchange.Stores
{
    public class DefaultTokenExchangeProviderStore : ITokenExchangeProviderStore
    {
        private readonly List<ExternalTokenExchangeProvider> _providers;

        public DefaultTokenExchangeProviderStore(IEnumerable<ExternalTokenExchangeProvider> providers)
        {
            _providers = providers.ToList();
        }
        public Task<ExternalTokenExchangeProvider> GetProviderByNameAsync(string name)
        {
            var provider = _providers.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (provider != null)
            {
                return Task.FromResult(provider);
            }

            return Task.FromResult<ExternalTokenExchangeProvider>(null);
        }

        public Task<List<ExternalTokenExchangeProvider>> GetProvidersAsync()
        {
            return Task.FromResult(_providers);
        }
    }

   

   
}

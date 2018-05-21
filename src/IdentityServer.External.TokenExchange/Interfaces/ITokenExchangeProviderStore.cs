using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Models;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface ITokenExchangeProviderStore
    {
        Task<ExternalTokenExchangeProvider> GetProviderByNameAsync(string name);
        Task<List<ExternalTokenExchangeProvider>> GetProvidersAsync();
    }
}

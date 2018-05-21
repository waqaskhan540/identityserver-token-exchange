using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Helpers;
using IdentityServer.External.TokenExchange.Interfaces;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Providers
{
    public class GoogleAuthProvider : IExternalTokenProvider
    {
        private readonly ITokenExchangeProviderStore _tokenExchangeProviderStore;
        private readonly HttpClient _client;
        public GoogleAuthProvider(ITokenExchangeProviderStore tokenExchangeProviderStore)
        {
            _tokenExchangeProviderStore = tokenExchangeProviderStore;
            _client = new HttpClient();
        }
        public async Task<JObject> GetUserInfoAsync(string accessToken)
        {

            var provider = await _tokenExchangeProviderStore.GetProviderByNameAsync(TokenExchangeProviders.Google);
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            var request = new Dictionary<string, string>();
            request.Add("token", accessToken);

            var result = await _client.GetAsync(provider.UserInfoEndpoint + QueryBuilder.GetQuery(request, TokenExchangeProviders.Google));
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Interfaces;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Providers
{
    public class LinkedInAuthProvider : IExternalTokenProvider
    {
        private readonly ITokenExchangeProviderStore _tokenExchangeProviderStore;
        private readonly HttpClient _client;
        public LinkedInAuthProvider(ITokenExchangeProviderStore tokenExchangeProviderStore)
        {
            _tokenExchangeProviderStore = tokenExchangeProviderStore;
            _client = new HttpClient();
        }
        public async Task<JObject> GetUserInfoAsync(string accessToken)
        {

            var provider = await _tokenExchangeProviderStore.GetProviderByNameAsync(TokenExchangeProviders.LinkedIn);
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var urlParams = $"oauth2_access_token={accessToken}&format=json";

            var result = await _client.GetAsync($"{provider.UserInfoEndpoint}{urlParams}");
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}

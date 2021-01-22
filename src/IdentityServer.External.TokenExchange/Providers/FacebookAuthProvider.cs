using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Helpers;
using IdentityServer.External.TokenExchange.Interfaces;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Providers
{
    public class FacebookAuthProvider : IExternalTokenProvider
    {
        private readonly ITokenExchangeProviderStore _tokenExchangeProviderStore;
        private readonly HttpClient _client;

        public FacebookAuthProvider(
            ITokenExchangeProviderStore tokenExchangeProviderStore
            )
        {
            _tokenExchangeProviderStore = tokenExchangeProviderStore;
            _client = new HttpClient();
        }
        public async Task<JObject> GetUserInfoAsync(string accessToken)
        {
            var provider = await _tokenExchangeProviderStore.GetProviderByNameAsync(TokenExchangeProviders.Facebook);
            ExternalTokenExchangeProviderHelper.CheckNotNull(provider);

            var request = new Dictionary<string, string>
            {
                {"fields", provider.Fields},
                {"access_token", accessToken}
            };
            var result = await _client.GetAsync(provider.UserInfoEndpoint + QueryBuilder.GetQuery(request, TokenExchangeProviders.Facebook));
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}

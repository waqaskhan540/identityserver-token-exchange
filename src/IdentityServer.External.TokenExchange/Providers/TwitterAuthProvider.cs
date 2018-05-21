using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Helpers;
using IdentityServer.External.TokenExchange.Interfaces;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Providers
{
    public class TwitterAuthProvider : IExternalTokenProvider
    {
        private readonly ITokenExchangeProviderStore _tokenExchangeProviderStore;
        private readonly HttpClient _client;
        public TwitterAuthProvider(ITokenExchangeProviderStore tokenExchangeProviderStore)
        {
            _tokenExchangeProviderStore = tokenExchangeProviderStore;
            _client = new HttpClient();
        }
        public async Task<JObject> GetUserInfoAsync(string accessToken)
        {
            var provider = await _tokenExchangeProviderStore.GetProviderByNameAsync(TokenExchangeProviders.Twitter);
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var request = new Dictionary<string, string>();
            request.Add("tokenString", accessToken);
            request.Add("endpoint", provider.UserInfoEndpoint);

            var authorizationHeaderParams = QueryBuilder.GetQuery(request, TokenExchangeProviders.Twitter);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderParams);

            var result = await _client.GetAsync(provider.UserInfoEndpoint);

            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}

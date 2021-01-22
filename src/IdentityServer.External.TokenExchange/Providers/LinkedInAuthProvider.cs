using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Helpers;
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
            ExternalTokenExchangeProviderHelper.CheckNotNull(provider);

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

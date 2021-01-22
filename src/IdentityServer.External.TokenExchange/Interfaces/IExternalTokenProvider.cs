using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IExternalTokenProvider
    {
        Task<JObject> GetUserInfoAsync(string accessToken);
    }
}

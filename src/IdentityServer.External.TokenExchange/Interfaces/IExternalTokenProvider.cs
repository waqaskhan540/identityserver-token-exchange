using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Models;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IExternalTokenProvider
    {
        Task<JObject> GetUserInfoAsync(string accessToken);
     
    }
}

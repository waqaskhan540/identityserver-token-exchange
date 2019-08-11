using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer.External.TokenExchange.Models;

namespace IdentityServer.External.TokenExchange.Config
{
    public static class Providers
    {
        public static IEnumerable<ExternalTokenExchangeProvider> GetProviders()
        {
            return new List<ExternalTokenExchangeProvider>
            {
                new ExternalTokenExchangeProvider
                {
                    Id = 1,
                    Fields = "id,email,name,gender,birthday",
                    Name = "Facebook",
                    UserInfoEndpoint = "https://graph.facebook.com/v2.8/me"
                },
                new ExternalTokenExchangeProvider
                {
                    Id = 2,
                    Name = "Google",
                    UserInfoEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo"
                },
                new ExternalTokenExchangeProvider
                {
                    Id = 3,
                    Name = "Twitter",
                    UserInfoEndpoint = "https://api.twitter.com/1.1/account/verify_credentials.json"
                }
               
            };
        }
    }
}

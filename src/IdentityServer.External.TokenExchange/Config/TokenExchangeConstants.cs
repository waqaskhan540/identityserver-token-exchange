using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.External.TokenExchange.Config
{
    public static class TokenExchangeProviders
    {
        public const string Facebook = "facebook";
        public const string LinkedIn = "linkedin";
        public const string Twitter = "twitter";
        public const string Google = "google";
        public const string GrantName = "external";
    }

    public static class TokenExchangeGrantParameters
    {
        public const string Provider = "provider";
        public const string ExternalToken = "external_token";
        public const string Email = "email";
    }

    public static class TokenExchangeErrors
    {
        public const string InvalidProvider = "invalid provider";
        public const string InvalidExternalToken = "invalid external token";
        public const string ProviderNotFound = "provider not found";
        public const string InvalidRequest = "invalid request";
        public const string UserNotCreated = "user could not be created, please try again.";
        public const string UserInfoNotRetrieved = "user info could not be retrieved from the given provider.";
    }
}

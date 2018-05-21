using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Processors
{
    public class NonEmailUserProcessor : INonEmailUserProcessor
    {
        private readonly IExternalUserStore _externalUserStore;
        public NonEmailUserProcessor(IExternalUserStore externalUserStore)
        {
            _externalUserStore = externalUserStore ?? throw new ArgumentNullException(nameof(externalUserStore));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider)
        {
            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                var existingUserId = await _externalUserStore.FindByProviderAsync(provider, userExternalId);
                if (string.IsNullOrWhiteSpace(existingUserId))
                {
                    var customResponse = new Dictionary<string, object>
                    {
                        { "userInfo", userInfo }
                    };
                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                                                     "could not retrieve user's email from the given provider, include email paramater and send request again.",
                                                    customResponse);
                }
                else
                {
                    var userClaims = await _externalUserStore.GetUserClaimsByExternalIdAsync(userExternalId);
                    return new GrantValidationResult(existingUserId, provider, userClaims, provider, null);
                }

            }

            var newUserId = await _externalUserStore.CreateExternalUserAsync(userExternalId, userEmail, provider);
            if (!string.IsNullOrWhiteSpace(newUserId))
            {
                var claims = await _externalUserStore.GetUserClaimsByExternalIdAsync(userExternalId);
                return new GrantValidationResult(newUserId, provider, claims, provider, null);
            }

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user, please try again.");

        }
    }
}

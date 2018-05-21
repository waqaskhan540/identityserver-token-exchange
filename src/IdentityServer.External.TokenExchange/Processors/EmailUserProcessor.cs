using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Processors
{
    public class EmailUserProcessor : IEmailUserProcessor
    {
        private readonly IExternalUserStore _externalUserStore;

        public EmailUserProcessor(IExternalUserStore externalUserStore)
        {
            _externalUserStore = externalUserStore;
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo,string email, string provider)
        {
            var userEmail = email;
            var userExternalId = userInfo.Value<string>("id");

            if (string.IsNullOrWhiteSpace(userExternalId))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user id from the token provided");
            }

            if (await _externalUserStore.FindUserByEmailAsync(userEmail))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "User with specified email already exists");

            }

            var newUserId = await _externalUserStore.CreateExternalUserAsync(userExternalId, userEmail, provider);
            if (!string.IsNullOrWhiteSpace(newUserId))
            {
                var claims = await _externalUserStore.GetUserClaimsByExternalIdAsync(userExternalId);
                return new GrantValidationResult(newUserId, provider, claims, provider, null);
            }
            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user , please try again.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer.External.TokenExchange.Helpers
{
    public static class GrantValidationResultHelpers
    {
        public static GrantValidationResult Error(string error)
        {
            return new GrantValidationResult(TokenRequestErrors.InvalidRequest,error);
        }

        public static GrantValidationResult Success(string userId, string provider, List<Claim> userClaims)
        {
            return new GrantValidationResult(userId,provider, userClaims, provider,null);
        }

       
    }
}

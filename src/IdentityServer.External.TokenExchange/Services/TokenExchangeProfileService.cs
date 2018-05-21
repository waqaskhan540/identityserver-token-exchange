using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdentityServer.External.TokenExchange.Services
{
    public class TokenExchangeProfileService : IProfileService
    {
        private readonly IExternalUserStore _externalUserStore;

        public TokenExchangeProfileService(IExternalUserStore externalUserStore)
        {
            _externalUserStore = externalUserStore;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            var subjectid = await _externalUserStore.FindByIdAsync(context.Subject.GetSubjectId());
            if (!string.IsNullOrWhiteSpace(subjectid))
            {
                var claims = await _externalUserStore.GetUserClaimsByIdAsync(subjectid);
                context.AddRequestedClaims(claims);
            }

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectid = await _externalUserStore.FindByIdAsync(context.Subject.GetSubjectId());
            context.IsActive = !string.IsNullOrWhiteSpace(subjectid);
        }
    }
}

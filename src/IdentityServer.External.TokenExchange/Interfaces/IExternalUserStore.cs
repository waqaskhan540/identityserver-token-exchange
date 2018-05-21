using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IExternalUserStore
    {
        Task<bool> FindUserByEmailAsync(string email);
        Task<bool> FindUserByExternalIdAsync(string externalId);
        Task<string> CreateExternalUserAsync(string externalId, string email, string provider);
        Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId);
        Task<List<Claim>> GetUserClaimsByIdAsync(string subjectid);


        Task<string> FindByProviderAsync(string provider, string externalId);

        Task<string> FindByIdAsync(string subject);
    }
}

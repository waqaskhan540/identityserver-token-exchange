using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer.External.TokenExchange.Models;
using IdentityServer4.Test;

namespace IdentityServer.External.TokenExchange.Stores
{
    public class DefaultExternalUserStore : IExternalUserStore
    {
        private readonly List<ExternalTestUser> _users;
        public DefaultExternalUserStore()
        {
                _users = new List<ExternalTestUser>();
        }
        public Task<bool> FindUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(x => x.Username == email);
            return Task.FromResult(user == null);

        }

        public Task<bool> FindUserByExternalIdAsync(string externalId)
        {
            var user = _users.FirstOrDefault(x => x.ProviderSubjectId == externalId);
            return Task.FromResult(user == null);
        }

        public Task<string> CreateExternalUserAsync(string externalId, string email, string provider)
        {
            var newUser = new ExternalTestUser
            {
                SubjectId = Guid.NewGuid().ToString(),
                ProviderSubjectId = externalId,
                Email = email,
                ProviderName = provider,
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email,email),
                    new Claim(JwtClaimTypes.Subject,externalId)
                }
            };
            _users.Add(newUser);

            return Task.FromResult(newUser.SubjectId);
        }

        public Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId)
        {
            var user = _users.FirstOrDefault(x => x.ProviderSubjectId == externalId);
            if (user != null)
            {
                return Task.FromResult(user.Claims.ToList());
            }

            return Task.FromResult(new List<Claim>());
        }

        public Task<string> FindByProviderAsync(string provider, string externalId)
        {
            var user = _users.FirstOrDefault(x => x.ProviderName.ToLower() == provider.ToLower()
                                                  && x.ProviderSubjectId == externalId);
            return Task.FromResult(user?.SubjectId);
        }

        public Task<string> FindByIdAsync(string subject)
        {
            var user = _users.FirstOrDefault(x => x.SubjectId == subject);
            if (user == null)
            {
                return null;
            }

            return Task.FromResult(user.SubjectId);
        }

        public Task<List<Claim>> GetUserClaimsByIdAsync(string subjectid)
        {
            var user = _users.FirstOrDefault(x => x.SubjectId == subjectid);
            if (user != null)
            {
                return Task.FromResult(user.Claims.ToList());
            }

            return Task.FromResult(new List<Claim>());
        }
    }
}


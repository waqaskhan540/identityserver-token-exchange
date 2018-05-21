using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IEmailUserProcessor 
    {
        Task<GrantValidationResult> ProcessAsync(JObject userInfo,string email, string provider);
    }
}

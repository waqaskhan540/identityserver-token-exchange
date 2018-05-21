using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Test;

namespace IdentityServer.External.TokenExchange.Models
{
    public class ExternalTestUser : TestUser
    {
      
        public string Email  { get; set; }
    }
}

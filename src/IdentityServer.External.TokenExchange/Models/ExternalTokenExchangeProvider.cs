using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.External.TokenExchange.Models
{
    public class ExternalTokenExchangeProvider
    {
        public int Id { get; set; }
        public string Fields { get; set; }
        public string Name { get; set; }
        public string UserInfoEndpoint { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IExternalTokenExchangeServiceBuilder
    {
        IServiceCollection Services { get; set; }

    }
}

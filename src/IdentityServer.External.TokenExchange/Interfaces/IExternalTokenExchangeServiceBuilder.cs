using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.External.TokenExchange.Interfaces
{
    public interface IExternalTokenExchangeServiceBuilder
    {
        IServiceCollection Services { get; set; }
    }
}

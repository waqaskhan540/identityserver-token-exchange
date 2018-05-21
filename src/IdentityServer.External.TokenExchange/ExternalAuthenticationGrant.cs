using System;
using System.Threading.Tasks;
using IdentityServer.External.TokenExchange.Config;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer.External.TokenExchange.Stores;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentityServer.External.TokenExchange.Helpers;

namespace IdentityServer.External.TokenExchange
{
    public class ExternalAuthenticationGrant : IExtensionGrantValidator
    {
        private readonly ITokenExchangeProviderStore _providerStore;
        private readonly Func<string, IExternalTokenProvider> _tokenServiceAccessor;
        private readonly IExternalUserStore _externalUserStore;
        private readonly IEmailUserProcessor _emailUserProcessor;
        private readonly INonEmailUserProcessor _nonEmailUserProcessor;


        public ExternalAuthenticationGrant(
            ITokenExchangeProviderStore providerStore,
            Func<string,IExternalTokenProvider> tokenServiceAccessor,
            IExternalUserStore externalUserStore,
            IEmailUserProcessor emailUserProcessor,
            INonEmailUserProcessor nonEmailUserProcessor
            )
        {
            _providerStore         = providerStore ?? throw new ArgumentNullException(nameof(providerStore));
            _tokenServiceAccessor  = tokenServiceAccessor ?? throw new ArgumentNullException(nameof(tokenServiceAccessor));
            _externalUserStore     = externalUserStore ?? throw new ArgumentNullException(nameof(externalUserStore));
            _emailUserProcessor    = emailUserProcessor ?? throw new ArgumentNullException(nameof(emailUserProcessor));
            _nonEmailUserProcessor = nonEmailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));
        }
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {

            try
            {
                var providerName = context.Request.Raw.Get(TokenExchangeGrantParameters.Provider);
                var externalToken = context.Request.Raw.Get(TokenExchangeGrantParameters.ExternalToken);
                var requestEmail = context.Request.Raw.Get(TokenExchangeGrantParameters.Email);

                if (string.IsNullOrWhiteSpace(providerName))
                {
                    context.Result = GrantValidationResultHelpers.Error(TokenExchangeErrors.InvalidProvider);
                    return;
                }

                if (string.IsNullOrWhiteSpace(externalToken))
                {
                    context.Result = GrantValidationResultHelpers.Error(TokenExchangeErrors.InvalidExternalToken);
                    return;
                }

                var provider = await _providerStore.GetProviderByNameAsync(providerName);
                if (provider == null)
                {
                    context.Result = GrantValidationResultHelpers.Error(TokenExchangeErrors.ProviderNotFound);
                    return;
                }

                var tokenService = _tokenServiceAccessor(providerName);
                var userInfo = await tokenService.GetUserInfoAsync(externalToken);
                if (userInfo == null)
                {
                    context.Result = GrantValidationResultHelpers.Error(TokenExchangeErrors.UserInfoNotRetrieved);
                    return;
                }

                var externalId = userInfo.Value<string>("id");
                if (!string.IsNullOrWhiteSpace(externalId))
                {
                    var existingUserId = await _externalUserStore.FindByProviderAsync(providerName,externalId);
                    if (!string.IsNullOrWhiteSpace(existingUserId))
                    {
                        var claims = await _externalUserStore.GetUserClaimsByExternalIdAsync(externalId);
                        context.Result = GrantValidationResultHelpers.Success(existingUserId,providerName,claims);
                    }
                }

                if (string.IsNullOrWhiteSpace(requestEmail))
                {
                    context.Result = await _nonEmailUserProcessor.ProcessAsync(userInfo, providerName);
                    return;
                }

                context.Result = await _emailUserProcessor.ProcessAsync(userInfo, requestEmail, providerName);
            }
            catch (Exception e)
            {

                context.Result = GrantValidationResultHelpers.Error(e.Message);
            }

        }

        public string GrantType => TokenExchangeProviders.GrantName;

      

        
    }


}

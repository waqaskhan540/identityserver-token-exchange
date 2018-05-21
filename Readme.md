# Exchanging external Tokens (Google, Twitter, Facebook,LinkedIn) with IdentityServer access tokens using an extension grant

## Installation

Package Manager
```
PM> Install IdentityServer.External.TokenExchange
```

## Setup
By default the package contains implementations for **Google** , **Facebook** , **Twitter** & **LinkedIn** and can be configured using the ```AddDefaultExternalTokenProviders``` method.

```C#
 services.AddIdentityServer()

                /** identity server configs **/

                .AddDeveloperSigningCredential()
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddTestUsers(IdentityServerConfig.GetUsers())

               /** token exchange configs **/
               
                .AddTokenExchangeForExternalProviders()  //registers an extension grant
                .AddDefaultTokenExchangeProviderStore()  //registers default in-memory store for providers info
                .AddDefaultExternalTokenProviders()      //registers providers auth implementations
                .AddDefaultTokenExchangeProfileService() //registers default profile service
                .AddDefaultExternalUserStore();          //registers default in-memory user's store
```



## Usage

* Request authentication using the provider's native library.
* Exchange external token with IdentityServer token by making following request to IdentityServer.

```
POST connect/token
     
     client_id = [your_client_id]
     client_secret = [your_client_secret]
     scopes = [your_scopes]
     grant_type = external
     provider = facebook 
     external_token  = [facebook_access_token]
```
 * If user is already registered then IdentityServer will return the access token, otherwise it will send the user's data and prompt for an email parameter to be added, in this case make another request with an extra ```email``` parameter.
 
 ```
POST connect/token
     
     client_id = [your_client_id]
     client_secret = [your_client_secret]
     scopes = [your_scopes]
     grant_type = external
     provider = facebook 
     email = myemail@abc.com
     external_token  = [facebook_access_token]
```

**You can change ```provider``` to ```Facebook``` , ```Google``` , ```Twitter``` and ```LinkedIn``` and provide respective token in the ```external_token``` parameter.**



## Customization

#### Adding Custom Provider
##### Step: 1 Provide external authentication provider
 Provide an implementation of ```IExternalTokenProvider```. 
This class will be responsible for talking to your external provider for retrieving user's info.
> The name of the class must follow the naming convention (Add "AuthProvider" at the end of your your class name) otherwise the DI would be unable to resolve it.

##### Step: 2 Provide a custom provider store 
Add a custom provider store by implementing ```ITokenExchangeProviderStore```. This class will be 
responsible for managing all information abour all the providers i.e. facebook , google and custom providers.

##### Step: 3 Register external provider 
Register your service in Startup.cs
```
 .AddCustomExternalTokenProvider<MyCustomAuthProvider>();
```
##### Step: 4 Register external provider store
Register your custom providers store.
```
  .AddCustomTokenExchangeProviderStore<MyCustomProviderStore>();
```

## License
Please see [LICENSE](/LICENSE) for licensing information.




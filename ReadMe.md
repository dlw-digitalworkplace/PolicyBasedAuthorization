# How to do authentication for a protected web api:

## Follow tutorials:

- [Protected web api](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview)
- [Setup oauth flow](https://dev.to/425show/calling-an-azure-ad-secured-api-with-postman-22co)

## Setup .NET CORE 6 api (template selector in VS 2022):

- Check: use MS identity
- Scaffolds some code in program.cs
- Swagger is provided by default, which is nice

`using Microsoft.Identity.Web; builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd")); app.UseAuthentication(); app.UseAuthorization(); app.MapControllers();`

## AAD app configuration:

- Platform web with redirect url to postman (https://oauth.pstmn.io/v1/callback) is important
- Expose an api

  - set default app uri
  - add scope (e.g. access_as_user)

- Not needed:
  - Does not need to be checked: Access tokens (used for implicit flows)
  - Does not need to be checked: ID tokens (used for implicit and hybrid flows)
  - Enable Allow public client flows
  - Adding client id to Expose an api > Authorized client applications

## Config AzureAd (secrets.json)

- client id
- tenant id
- instance (= https://login.microsoftonline.com/)

These are the only Azure AD settings that your web api requires for the current setup.

`

    {
      "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "ClientId": "",
        "TenantId": ""
      },
      "Logging": {
        "LogLevel": {
          "Default": "Warning"
        }
      },
      "AllowedHosts": "*"
    }

`

## Postman: Oauth 2.0 Authorization code flow

Fill out all parameters correctly! Use the Postman console for debugging purposes.

- Grant type: Authorization code
- Callback url: https://oauth.pstmn.io/v1/callback
- Check Authorize using browser
- Auth url: get from AAD app > Overview > Endpoints (login.msonline.com/.../authorize)
- Access token url: get from AAD app > Overview > Endpoints (login.msonline.com/.../token)
- scope: api://<clientid>/<scope_name>
- Client id
- Client secret
- State: <empty>
- enable popup in browser when prompted
- open in browser where you are already logged in to the specific tenant for easy login
- use the token
- hit send on whatever GET request towards localhost you want to execute (e.g. WeatherController)

# How to do authorization for a web api:

## General authorization stuff:

- Add [Authorize] attribute. This will only verify if user is authenticated
- Add [RequiredScope("access_as_user")] to only allow if access_as_user scope is present in JWT token
- Add [Authorize(Roles = "Administrator")] to use role based authorization

## Policy based authorization:

- [MS docs](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0) provide good info
- Some interfaces are implemented to be able to let a handler be reused across multiple requirements

## Code scaffolding:

- [Authorize(Policy = "MyPolicy")] for policy based authorization on controllers / controller methods

- Program.cs (or the former Startup.cs)

  - Inject your custom authorization handlers as Scoped in services or you will get a 403 response without error message

    `builder.Services.AddScoped<IAuthorizationHandler, MyHandler>(); builder.Services.AddAuthorization(options => { options.AddPolicy("MyPolicy", p => p.AddRequirements(new MyRequirement())); });`

  - Create a MyRequirement.cs file (blank class, implements IAuthorizationRequirement)
  - Create a MyHandler.cs file (implements AuthorizationHandler<MyRequirement>)
    - override async Task HandleRequirementAsync (contains the logic to let the authorization fail or succeed)

# Useful links

- [Tutorial protected web api](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview)
- [Fetch access token using the oauth 2.0 authorization code flow in Postman](https://dev.to/425show/calling-an-azure-ad-secured-api-with-postman-22co)
- [Policy-based authorization (MS docs)](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0)

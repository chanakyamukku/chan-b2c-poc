using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);
        }

        public static string AadInstance = "https://login.microsoftonline.com/tfp/{0}/{1}/v2.0/.well-known/openid-configuration";
        public static string Tenant = "chantesting.onmicrosoft.com";
        public static string ClientId = "5e543884-0f6c-4ab2-a74b-87754a885e5b";
        public static string SignUpSignInPolicy = "B2C_1_chantestsignup";

        /*
         * Configure the authorization OWIN middleware.
         */
        public void ConfigureAuth(IAppBuilder app)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // Accept only those tokens where the audience of the token is equal to the client ID of this app
                ValidAudience = ClientId,
                AuthenticationType = SignUpSignInPolicy,
            };

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C metadata & signing keys from the OpenIDConnect metadata endpoint
                AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(String.Format(AadInstance, Tenant, SignUpSignInPolicy)))
            });
        }
    }
}

using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace hangfiredemo
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
      

        //private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string HangFireCookieName = "HangFireCookie";
        private static readonly int CookieExpirationMinutes = 60;
        private TokenValidationParameters tokenValidationParameters;
        private string role;

        public HangfireAuthorizationFilter(TokenValidationParameters tokenValidationParameters, string role = null)
        {
            this.tokenValidationParameters = tokenValidationParameters;
            this.role = role;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var access_token = String.Empty;
            var setCookie = false;

            // try to get token from query string
            if (httpContext.Request.Query.ContainsKey("access_token"))
            {
                access_token = httpContext.Request.Query["access_token"].FirstOrDefault();
                setCookie = true;
            }
            else
            {
                access_token = httpContext.Request.Cookies[HangFireCookieName];
            }

            if (String.IsNullOrEmpty(access_token))
            {
                return false;
            }

            try
            {
                SecurityToken validatedToken = null;
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var claims = hand.ValidateToken(access_token, this.tokenValidationParameters, out validatedToken);
                if (String.IsNullOrEmpty(this.role) && !claims.IsInRole(this.role))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                //logger.Error(e, "Error - Invalid jwt token/validation process");
                System.Diagnostics.Debug.WriteLine(e + "-" + "Error - Invalid jwt token / validation process");
                //the method that calls this method(Authorize method) should handle the exception 
                throw e;
                
                
            }

            if (setCookie)
            {
                httpContext.Response.Cookies.Append(HangFireCookieName,
                access_token,
                new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(CookieExpirationMinutes)
                });
            }


            return true;
        }
    }
}

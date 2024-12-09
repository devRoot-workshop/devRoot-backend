using System;
using System.Net;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using devRoot.Server.Models;

namespace devRoot.Server.Auth
{
    public static class AuthGlobal
    {
        public static Dictionary<FirebaseToken, List<Role.RoleType>> cachedTokens { get; set; } = new Dictionary<FirebaseToken, List<Role.RoleType>>();
    }

    public class FirebaseAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token).Result;
                context.HttpContext.Items["User"] = decodedToken;
                Console.WriteLine(decodedToken.Uid);
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public class FirebaseService
    {
        public FirebaseService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("DEVROOTFIREBASESTRING", EnvironmentVariableTarget.Machine))
                });
            }
        }
    }
}

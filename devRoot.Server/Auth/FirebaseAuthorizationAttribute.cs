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
using Microsoft.AspNetCore.Mvc.Authorization;

namespace devRoot.Server.Auth
{
    public static class AuthGlobal
    {
        public static Dictionary<FirebaseToken, List<Role.RoleType>> cachedTokens { get; set; } = new Dictionary<FirebaseToken, List<Role.RoleType>>();
    }

    public class FirebaseAuthorizationAttribute : TypeFilterAttribute
    {
        public FirebaseAuthorizationAttribute(AuthorizationMode authorizationMode) : base(typeof(FirebaseAuthorizationAttributeFilter))
        {
            Arguments = new object[] { authorizationMode };
        }

        public class FirebaseAuthorizationAttributeFilter : IAuthorizationFilter
        {
            private readonly AuthorizationMode _authorizationMode;

            public FirebaseAuthorizationAttributeFilter(AuthorizationMode authorizationMode)
            {
                _authorizationMode = authorizationMode;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                try
                {
                    if (Environment.GetEnvironmentVariable("DEVROOTDEBUG", EnvironmentVariableTarget.Machine) != "TRUE")
                    {
                        var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

                        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                        {
                            if (_authorizationMode == AuthorizationMode.Mandatory)
                            {
                                context.Result = new UnauthorizedResult();
                            }
                            else
                            {
                                return;
                            }
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
                            if (_authorizationMode == AuthorizationMode.Mandatory)
                            {
                                context.Result = new UnauthorizedResult();
                            }
                            else
                            {
                                context.Result = new OkResult();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
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

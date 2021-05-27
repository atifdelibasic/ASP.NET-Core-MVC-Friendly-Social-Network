using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataLayer.EntityModels;

namespace FriendlyRS1.Helpers
{
    public class MiddlewareHelper
    {
        public class SomeMiddleware
        {
            private readonly RequestDelegate _next;
            public SomeMiddleware(RequestDelegate next, SignInManager<ApplicationUser> signInManager)
            {
                _next = next;
            }

            //public async Task InvokeAsync(HttpContext httpContext)
            //{
            //    if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            //    {
            //        var claims = new List<Claim>
            //    {
            //    new Claim("SomeClaim", "SomeValue")
            //    };

            //        var appIdentity = new ClaimsIdentity(claims);
            //        httpContext.User.AddIdentity(appIdentity);
            //    }

            //    await _next(httpContext);
            //}


        }
    }
}

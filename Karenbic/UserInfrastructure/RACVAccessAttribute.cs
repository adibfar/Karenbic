using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.UserInfrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RACVAccessAttribute : AuthorizeAttribute
    {
        public string Issuer { get; set; }
        public string ClaimType { get; set; }
        public string Value { get; set; }


        protected override bool AuthorizeCore(HttpContextBase context)
        {
            if (!string.IsNullOrEmpty(Issuer) && !string.IsNullOrEmpty(ClaimType) && !string.IsNullOrEmpty(Value))
            {
                return context.User.Identity.IsAuthenticated
                    && context.User.Identity is ClaimsIdentity
                    && ((ClaimsIdentity)context.User.Identity).HasClaim(x =>
                    x.Issuer == Issuer && x.Type == ClaimType && x.Value == Value
                );
            }
            else
            {
                return base.AuthorizeCore(context);
            }

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //throw new UnauthorizedAccessException(); //to avoid multiple redirects
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.HttpContext.Response.Redirect("/Account/Login");
            }
            else
            {
                handleAjaxRequest(filterContext);
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private static void handleAjaxRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (!ctx.Request.IsAjaxRequest())
                return;

            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            ctx.Response.End();
        }
    }
}
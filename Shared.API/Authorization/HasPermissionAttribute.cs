using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _requiredPermission;

        public HasPermissionAttribute(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userPermissions = user.FindAll("permission").Select(p => p.Value);

            if (!userPermissions.Contains(_requiredPermission))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

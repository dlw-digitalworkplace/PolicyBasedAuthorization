using DLWR.PolicyBasedAuthorization.NET6.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DLWR.PolicyBasedAuthorization.NET6.Authorization.Handlers
{
    internal class MyHandler : AuthorizationHandler<MyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ////// HANDLER IS CALLED WHENEVER A POLICY IS TRIGGERED THAT IS TIED TO THIS TYPE OF REQUIREMENT //////
            MyRequirement requirement
            ////// HANDLER IS CALLED WHENEVER A POLICY IS TRIGGERED THAT IS TIED TO THIS TYPE OF REQUIREMENT //////
        )
        {
            await Task.CompletedTask;

            var upn = context.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(upn))
            {
                // User cannot be verified
                context.Fail();
                return;
            }
            else
            {
                var request = _httpContextAccessor?.HttpContext?.Request;

                try
                {
                    var isOk = true;


                    if (isOk)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {

                        context.Fail();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Fail();
                    return;
                }
            }
        }
    }
}

using DLWR.PolicyBasedAuthorization.NET6.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DLWR.PolicyBasedAuthorization.NET6.Authorization.Handlers
{
    internal class IsWeatherManHandler : AuthorizationHandler<WorksWithCloudsRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsWeatherManHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ////// HANDLER IS CALLED WHENEVER A POLICY IS TRIGGERED THAT IS TIED TO THIS TYPE OF REQUIREMENT //////
            ///
            /// A requirement can have multiple handlers. A handler may inherit AuthorizationHandler<TRequirement>,
            /// where TRequirement is the requirement to be handled.Alternatively, a handler may implement
            /// IAuthorizationHandler directly to handle more than one type of requirement.
            ///
            WorksWithCloudsRequirement requirement // todo: add IsWeatherManRequirement
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
                if (upn.Contains("frank"))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    // we don't execute context.Fail() in this case, other handlers might validate the requirement
                }
            }
        }
    }
}

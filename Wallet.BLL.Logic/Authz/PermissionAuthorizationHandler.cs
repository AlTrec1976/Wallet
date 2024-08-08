using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wallet.BLL.Logic.Auth;
using Wallet.BLL.Logic.Contracts.Permissions;
using Wallet.Common.Entities.Authz;

namespace Wallet.BLL.Logic.Authz
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, ILogger<PermissionAuthorizationHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            try
            {
                var userId = context.User.Claims.FirstOrDefault(
                    c => c.Type == CustomClaims.UserId);

                Guid.TryParse(userId?.Value, out var id);

                using var scope = _serviceScopeFactory.CreateScope();
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionLogic>();

                var permissions = await permissionService.GetPermissionsAsync(id);

                if (permissions.Intersect(requirement.Permissions).Any())
                {

                    context.Succeed(requirement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в HandleRequirementAsync");
                throw;
            }
        }
    }
}

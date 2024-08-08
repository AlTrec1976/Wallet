using Microsoft.AspNetCore.Authorization;

namespace Wallet.Common.Entities.Authz
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string policyName;
        public PermissionRequirement(Permission[] permissions)
        {
            Permissions = permissions;
        }
        public Permission[] Permissions { get; set; }
    }
}

using Wallet.Common.Entities.Authz;

namespace Wallet.BLL.Logic.Contracts.Permissions
{
    public interface IPermissionLogic
    {
        Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
    }
}

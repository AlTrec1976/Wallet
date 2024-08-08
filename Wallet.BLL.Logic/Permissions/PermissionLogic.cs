using Wallet.Common.Entities.Authz;
using Wallet.DAL.Repository;
using Wallet.DAL.Repository.Contracts;

namespace Wallet.BLL.Logic.Contracts.Permissions
{
    public class PermissionService : IPermissionLogic
    {
        private readonly IUserRepository _userRepository;

        public PermissionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<HashSet<Permission>> GetPermissionsAsync(Guid userId)
        {
            return await _userRepository.GetUserPermission(userId);
        }
    }
}

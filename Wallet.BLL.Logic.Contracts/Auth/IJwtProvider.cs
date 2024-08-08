using Wallet.Common.Entities.User.DB;

namespace Wallet.BLL.Logic.Contracts.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}

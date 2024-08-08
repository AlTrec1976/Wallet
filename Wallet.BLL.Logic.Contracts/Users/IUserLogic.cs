using Wallet.Common.Entities.User.DB;
using Wallet.Common.Entities.User.InputModels;

namespace Wallet.BLL.Logic.Contracts.Users
{
    public interface IUserLogic
    {
        Task CreateUserAsync(UserCreateInputModel userInputModel);

        Task<List<User>> Get();

        Task<User> Get(Guid id);

        Task<string> LoginAsync(UserCreateInputModel userInputModel);
    }
}

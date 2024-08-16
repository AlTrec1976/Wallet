using Wallet.Common.Entities.User.InputModels;

namespace Wallet.Common.Entities.UserModels.InputModels
{
    public class UserUpdateInputModel : UserCreateInputModel
    {
        public Guid Id { get; set; }
    }
}

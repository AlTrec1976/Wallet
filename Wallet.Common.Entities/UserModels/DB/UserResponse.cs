namespace Wallet.Common.Entities.UserModels.DB
{
    public record UserResponse : UserRequest
    {
        public Guid Id { get; set; }
    }
}

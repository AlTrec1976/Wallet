namespace Wallet.Common.Entities.Auth
{
    public interface IJwtOptions
    {
        string SecretKey { get; set; }

        int ExpiresHours { get; set; }

        string Issuer { get; set; }

        string Audience { get; set; }
    }
}

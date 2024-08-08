namespace Wallet.Common.Entities.Auth
{
    public class JwtOptions : IJwtOptions
    {
        //секретный ключ
        public string SecretKey { get; set; } = string.Empty;
        //сколько часов будет действовать токен
        public int ExpiresHours { get; set; } = default;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}

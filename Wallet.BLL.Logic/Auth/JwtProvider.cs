using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wallet.BLL.Logic.Contracts.Auth;
using Wallet.Common.Entities.Auth;
using Wallet.Common.Entities.User.DB;

namespace Wallet.BLL.Logic.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IOptions<JwtOptions> _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim> { new Claim(CustomClaims.UserId, user.Id.ToString()) };

            //алгоритм кодирования
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            //создаем токен
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                expires: DateTime.UtcNow.AddHours(_options.Value.ExpiresHours));

            //получаем строку из JwtSecurityToken
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }


    }
}

using BenzForum.Data.ModelsIn;
using BenzForum.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BenzForum.Helpers
{
    public class JwtUtils : IJwtUtils
    {
        private static readonly double ExpiresInHours = 1;
        private readonly IConfiguration _configuration;

        private const string JwtKeyConfig = "Jwt:Key";
        private const string JwtIssuerConfig = "Jwt:Issuer";
        private const string JwtAudienceConfig = "Jwt:Audience";

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateToken(DBUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration[JwtKeyConfig] ?? throw new InvalidOperationException("JWT key is not configured."));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(ExpiresInHours),
                Issuer = _configuration[JwtIssuerConfig],
                Audience = _configuration[JwtAudienceConfig],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}



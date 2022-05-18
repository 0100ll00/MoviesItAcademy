using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Models.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesItAcademy.Service.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly int _expirationDateInMinutes;

        public JwtService(IOptions<JwtConfiguration> options)
        {
            _secret = options.Value.Secret;
            _expirationDateInMinutes = options.Value.ExpirationInMinutes;
        }

        public string GenerateSecurityToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_expirationDateInMinutes),
                Audience = "localhost",
                Issuer = "localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

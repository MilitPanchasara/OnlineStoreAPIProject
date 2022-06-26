using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        Dictionary<string, string> UsersRecords = new Dictionary<string, string>
        {
            { "user1","password1"},
            { "user2","password2"},
            { "user3","password3"},
        };

        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }

        public Token Authenticate(User users)
        {
            if (!UsersRecords.Any(x => x.Key == users.FirstName && x.Value == users.Password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, users.FirstName),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { Token_ = tokenHandler.WriteToken(token) };
        }
    }

    public interface IJWTManagerRepository
    {
        Token Authenticate(User users);
    }
}

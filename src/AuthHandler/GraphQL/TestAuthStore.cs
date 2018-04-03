using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthHandler.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthHandler.GraphQL
{
    public class TestAuthStore : IAuthStore
    {
        private IConfiguration _configuration;

        public TestAuthStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Auth> GetAuthTokenAsync(string type, string token)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "bob")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new Auth()
            {
                Type = type,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };

        }
    }
}
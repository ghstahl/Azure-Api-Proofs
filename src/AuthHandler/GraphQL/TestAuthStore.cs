using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthHandler.Google;
using AuthHandler.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthHandler.GraphQL
{
    public class TestBindStore : IBindStore
    {
        private IConfiguration _configuration;
        private JwtTokenValidation _jwtTokenValidation;
        public TestBindStore(IConfiguration configuration, JwtTokenValidation jwtTokenValidation)
        {
            _configuration = configuration;
            _jwtTokenValidation = jwtTokenValidation;
        }

        public async Task<BindResult> BindAsync(string type, string token)
        {
            ClaimsPrincipal cp;
            switch (type)
            {
                case "google-id_token":
                case "norton-id_token":
                    // These will throw, but best to let it happen and catch all the way up top.
                    cp = await _jwtTokenValidation.ValidateToken(type,token);
                    break;
            }
            

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
            return new BindResult()
            {
                Type = type,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                SPOCEntity = Guid.NewGuid().ToString()
            };
        }
    }
}
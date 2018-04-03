using System.Threading.Tasks;
using AuthHandler.Models;

namespace AuthHandler.GraphQL
{
    public class TestAuthStore : IAuthStore
    {
        public async Task<Auth> GetAuthTokenAsync(string type, string token)
        {
            return new Auth()
            {
                Type = type,
                Token = token
            };

        }
    }
}
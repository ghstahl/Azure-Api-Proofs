using System.Threading.Tasks;
 namespace AuthHandler.Models
{
    public interface IAuthStore  
    {

        /// <summary>
        /// Gets the tenantId of this store.
        /// </summary>
        /// <returns></returns>
        Task<Auth> GetAuthTokenAsync(string type, string token);
    }
}
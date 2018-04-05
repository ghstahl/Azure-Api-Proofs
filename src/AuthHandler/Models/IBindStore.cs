using System.Threading.Tasks;
 namespace AuthHandler.Models
{
    public interface IBindStore  
    {

        /// <summary>
        /// Gets the tenantId of this store.
        /// </summary>
        /// <returns></returns>
        Task<BindResult> BindAsync(string type, string token);
    }
}
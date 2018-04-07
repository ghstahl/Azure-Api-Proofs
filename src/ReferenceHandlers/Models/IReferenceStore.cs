using System.Threading.Tasks;

namespace ReferenceHandlers.Models
{
    public interface IReferenceStore  
    {

        /// <summary>
        /// Gets the tenantId of this store.
        /// </summary>
        /// <returns></returns>
        Task<int> AddAsync(int v1, int v2);
    }
} 
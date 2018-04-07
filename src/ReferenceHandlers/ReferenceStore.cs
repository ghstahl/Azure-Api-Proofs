using System;
using System.Threading.Tasks;
using ReferenceHandlers.Models;

namespace ReferenceHandlers
{
    public class ReferenceStore : IReferenceStore
    {
        public async Task<int> AddAsync(int v1, int v2)
        {
            return v1 + v2;
        }
    }
}

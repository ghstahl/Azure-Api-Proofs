using System.Collections.Generic;
using System.Security.Claims;

namespace AuthHandler.Models
{
    public class IdentityModel
    {
        public List<ClaimHandle> Claims { get; set; }
    }
}
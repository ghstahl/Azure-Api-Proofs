using AuthHandler.Models;
using GraphQL.Types;

namespace AuthHandler.GraphQL
{
    public class IdentityModelType : ObjectGraphType<Models.IdentityModel>
    {
        public IdentityModelType()
        {
            Name = "identity";
            Field<ListGraphType<ClaimHandleType>>("claims", "The Claims of the identity");
        }
    }
}
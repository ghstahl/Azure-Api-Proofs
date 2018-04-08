using AuthHandler.Models;
using GraphQL.Types;

namespace AuthHandler.GraphQL
{
    public class ClaimHandleType : ObjectGraphType<ClaimHandle>
    {
        public ClaimHandleType()
        {
            Name = "claim";
        
            Field(x => x.Name).Description("name of claim.");
            Field(x => x.Value).Description("value of claim.");
        }
    }
}
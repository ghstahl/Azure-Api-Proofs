using AuthHandler.Models;
using GraphQL.Types;
 

namespace AuthHandler.GraphQL
{
 
    public class AuthType : ObjectGraphType<Auth>
    {
        public AuthType()
        {
            Name = "auth";
            Field(x => x.Type).Description("The Auth Token Type.");
            Field(x => x.Token).Description("The Auth Token.");
        }
    }
}
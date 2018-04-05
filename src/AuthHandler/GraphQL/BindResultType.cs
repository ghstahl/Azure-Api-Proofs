using AuthHandler.Models;
using GraphQL.Types;
 

namespace AuthHandler.GraphQL
{
    public class BindResultType : ObjectGraphType<BindResult>
    {
        public BindResultType()
        {
            Name = "bindResult";
            Field(x => x.Type).Description("The Auth Token Type.");
            Field(x => x.Token).Description("The Auth Token.");
            Field(x => x.SPOCEntity).Description("The SPOC Entity.");
        }
    }
}

using GraphQL.Types;

namespace AuthHandler.GraphQL
{
    public class BindInput : InputObjectGraphType
    {
        public BindInput()
        {
            Name = "bindInput";
            Field<NonNullGraphType<StringGraphType>>("type");
            Field<NonNullGraphType<StringGraphType>>("token");
        }
    }
}
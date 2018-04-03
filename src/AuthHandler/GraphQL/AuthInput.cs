using GraphQL.Types;

namespace AuthHandler.GraphQL
{
    public class AuthInput : InputObjectGraphType
    {
        public AuthInput()
        {
            Name = "authInput";
            Field<NonNullGraphType<StringGraphType>>("type");
            Field<NonNullGraphType<StringGraphType>>("token");
        }
    }
}
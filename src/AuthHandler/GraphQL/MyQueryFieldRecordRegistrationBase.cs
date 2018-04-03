using System;
using AuthHandler.Models;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace AuthHandler.GraphQL
{
    public class MyQueryFieldRecordRegistrationBase : IQueryFieldRecordRegistration
    {
        private IAuthStore _authStore;

        public MyQueryFieldRecordRegistrationBase(
            IAuthStore authStore)
        {
            _authStore = authStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldName = "auth";
            var fieldType = queryCore.FieldAsync<AuthType>(name: fieldName,
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AuthInput>> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var input = context.GetArgument<Auth>("input");
                       
                        var result = await _authStore.GetAuthTokenAsync(input.Type,input.Token);
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
        }
    }
}

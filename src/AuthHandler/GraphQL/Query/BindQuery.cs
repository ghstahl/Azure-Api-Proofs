using System;
using AuthHandler.Models;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace AuthHandler.GraphQL.Query
{
    public class BindQuery : IQueryFieldRecordRegistration
    {
        
        private IBindStore _bindStore;
        public BindQuery(IBindStore bindStore)
        {
            _bindStore = bindStore;
        }
 
        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<BindResultType>(name: "bind",
                description: "Given a type of token, we will exchange it for an oAuth2 token to access this service.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BindInput>> { Name = "input" }),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var input = context.GetArgument<BindInputHandle>("input");

                        var result = await _bindStore.BindAsync(input.Type, input.Token);
                        return result;
                    }
                    catch (Exception e)
                    {
                        context.Errors.Add(new ExecutionError("Unable to bind with giving input"));
                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
        }
    }
}
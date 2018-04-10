using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace AuthHandler.GraphQL.Mutation
{
    public class SomeRandomMutation : IMutationFieldRecordRegistration
    {
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            /*(
            mutationCore.FieldAsync<StringGraphType>(name: "someRandomMutation",
                description: null,
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var id = context.GetArgument<string>("id");

                        return true;
                    }
                    catch (Exception e)
                    {

                    }
                    return false;
                    //                    return await Task.Run(() => { return ""; });

                },
                deprecationReason: null
            );
            */
        }
    }
}
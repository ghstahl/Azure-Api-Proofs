using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL.Query
{
    public class HumanQuery : IQueryFieldRecordRegistration
    {
        private StarWarsData _starWarsData;
        public HumanQuery(StarWarsData starWarsData)
        {
            _starWarsData = starWarsData;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldType = queryCore.FieldAsync<HumanType>(
                name: "human",
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>
                    { Name = "id", Description = "id of the human" }),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var human = await _starWarsData.GetHumanByIdAsync(context.GetArgument<string>("id"));
                        return human;
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
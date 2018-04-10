using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL.Mutation
{
    public class CreateHumanMutation : IMutationFieldRecordRegistration
    {
        private StarWarsData _starWarsData;

        public CreateHumanMutation(
            StarWarsData starWarsData )
        {
            _starWarsData = starWarsData;
        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<HumanType>(name: "createHuman",
                description: null,
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<HumanInputType>> { Name = "human" }
                ),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var human = context.GetArgument<Human>("human");
                        return _starWarsData.AddHuman(human);
                    }
                    catch (Exception e)
                    {

                    }
                    return false;
                    //                    return await Task.Run(() => { return ""; });

                },
                deprecationReason: null
            );
        }
    }
}
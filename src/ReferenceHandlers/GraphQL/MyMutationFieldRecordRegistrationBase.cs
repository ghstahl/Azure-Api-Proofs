using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL
{
    public class MyMutationFieldRecordRegistrationBase : IMutationFieldRecordRegistration
    {
        private IReferenceStore _referenceStore;
        private StarWarsData _starWarsData;

        public MyMutationFieldRecordRegistrationBase(
            StarWarsData starWarsData,
            IReferenceStore referenceStore)
        {
            _starWarsData = starWarsData;
            _referenceStore = referenceStore;
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
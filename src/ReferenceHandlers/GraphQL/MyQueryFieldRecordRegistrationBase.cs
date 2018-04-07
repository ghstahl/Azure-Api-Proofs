using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL
{
    public class MyQueryFieldRecordRegistrationBase : IQueryFieldRecordRegistration
    {
        private IReferenceStore _referenceStore;
        private StarWarsData _starWarsData;

        public MyQueryFieldRecordRegistrationBase(
            StarWarsData starWarsData,
            IReferenceStore referenceStore)
        {
            _starWarsData = starWarsData;
            _referenceStore = referenceStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldName = "human";
            var fieldType = queryCore.FieldAsync<HumanType>(
                name: fieldName,
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

            fieldName = "hero";
            fieldType = queryCore.FieldAsync<CharacterInterface>(
                name: fieldName,
                description: null,
                resolve: async context =>
                {
                    var result = await _starWarsData.GetDroidByIdAsync("3");
                    return result;
                });

            Func<ResolveFieldContext, string, object> func = (context, id) => _starWarsData.GetDroidByIdAsync(id);

            fieldName = "droid";
            fieldType = queryCore.FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                ),
                resolve: func
            );
        }
    }
}

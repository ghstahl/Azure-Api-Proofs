using System;
using GraphQL.Types;
using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL.Query
{
    public class DroidQuery : IQueryFieldRecordRegistration
    {

        private StarWarsData _starWarsData;

        public DroidQuery(
            StarWarsData starWarsData)
        {
            _starWarsData = starWarsData;

        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            Func<ResolveFieldContext, string, object> func = (context, id) => _starWarsData.GetDroidByIdAsync(id);

            queryCore.FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "id", Description = "id of the droid"}
                ),
                resolve: func
            );
        }
    }
}

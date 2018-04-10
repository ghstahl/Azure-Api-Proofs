using P7.GraphQLCore;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers.GraphQL.Query
{
    public class HeroQuery : IQueryFieldRecordRegistration
    {
        private StarWarsData _starWarsData;
        public HeroQuery(StarWarsData starWarsData)
        {
            _starWarsData = starWarsData;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldType = queryCore.FieldAsync<CharacterInterface>(
                name: "hero",
                description: null,
                resolve: async context =>
                {
                    var result = await _starWarsData.GetDroidByIdAsync("3");
                    return result;
                });
        }
    }
}
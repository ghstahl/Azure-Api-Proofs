using GraphQL.Types;

namespace ReferenceHandlers.Types
{
    public class HumanInputType : InputObjectGraphType
    {
        public HumanInputType()
        {
            Name = "HumanInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("homePlanet");
        }
    }
}
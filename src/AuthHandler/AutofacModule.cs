using AuthHandler.Google;
using AuthHandler.GraphQL;
using AuthHandler.GraphQL.Mutation;
using AuthHandler.GraphQL.Query;
using AuthHandler.Models;
using Autofac;
using P7.Core;
using P7.GraphQLCore;


namespace AuthHandler
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BindInput>();
            builder.RegisterType<BindResultType>();
            builder.RegisterType<BindInputHandle>();

            builder.RegisterType<IdentityModelType>();
            builder.RegisterType<ClaimHandleType>();

            builder.RegisterType<TestBindStore>().As<IBindStore>();

            builder.RegisterType<IdentityQuery>().As<IQueryFieldRecordRegistration>();
            builder.RegisterType<BindQuery>().As<IQueryFieldRecordRegistration>();
            builder.RegisterType<SomeRandomMutation>().As<IMutationFieldRecordRegistration>();

            builder.RegisterType<JwtTokenValidation>().SingleInstance();
            builder.RegisterType<GoogleDiscoverCacheContainer>().SingleInstance();
            builder.RegisterType<NortonDiscoverCacheContainer>().SingleInstance();

        }
    }
}
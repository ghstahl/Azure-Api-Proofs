using Autofac;
using P7.GraphQLCore.Stores;
using TheApiApp.Core.Controllers;

namespace TheApiApp.Core
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var env = P7.Core.Global.HostingEnvironment;


            builder.RegisterType<InMemoryGraphQLFieldAuthority>()
                .As<InMemoryGraphQLFieldAuthority>()
                .As<IGraphQLFieldAuthority>()
                .SingleInstance();

            builder.RegisterType<MyDatabase>().As<IMyDatabase>();
        }
    }
}

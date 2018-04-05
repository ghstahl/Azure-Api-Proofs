using AuthHandler.GraphQL;
using AuthHandler.Models;
using Autofac;
 

namespace AuthHandler
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BindInput>();
            builder.RegisterType<BindResultType>();
            builder.RegisterType<BindInputHandle>();
            builder.RegisterType<TestBindStore>().As<IBindStore>();
        }
    }
}
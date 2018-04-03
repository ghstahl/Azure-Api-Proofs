using AuthHandler.GraphQL;
using AuthHandler.Models;
using Autofac;
 

namespace AuthHandler
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthInput>();
            builder.RegisterType<AuthType>();
            builder.RegisterType<Auth>();
            builder.RegisterType<TestAuthStore>().As<IAuthStore>();
        }
    }
}
using Autofac;
using Microsoft.Extensions.Hosting;
using P7.Core.Scheduler.Scheduling;
using P7.GraphQLCore.Stores;
using TheApiApp.Core.Controllers;
using TheApiApp.Core.Scheduler;

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
            builder.RegisterType<DummyHealthScheduledTask>().As<IScheduledTask>();
            builder.RegisterType<SchedulerHostedService>()
                .As<IHostedService>()
                .SingleInstance();


        }
    }
}

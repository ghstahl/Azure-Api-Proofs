using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using P7.BlogStore.Hugo.Extensions;
using P7.GraphQLCore.Stores;

namespace ContactListAspNetCore
{
    public class AutofacModule : Module
    {
        private static string TenantId = "02a6f1a2-e183-486d-be92-658cd48d6d94";

        protected override void Load(ContainerBuilder builder)
        {
            var env = P7.Core.Global.HostingEnvironment;
            var dbPath = Path.Combine(env.ContentRootPath, "App_Data/blogstore");
            Directory.CreateDirectory(dbPath);
            builder.AddBlogStoreBiggyConfiguration(dbPath, TenantId);

            builder.RegisterType<InMemoryGraphQLFieldAuthority>()
                .As<InMemoryGraphQLFieldAuthority>()
                .As<IGraphQLFieldAuthority>()
                .SingleInstance();
        }
    }
}

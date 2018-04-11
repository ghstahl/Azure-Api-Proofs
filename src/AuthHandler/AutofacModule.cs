using System.Collections.Generic;
using AuthHandler.Google;
using AuthHandler.GraphQL;
using AuthHandler.GraphQL.Mutation;
using AuthHandler.GraphQL.Query;
using AuthHandler.Models;
using Autofac;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using P7.Core;
using P7.GraphQLCore;


namespace AuthHandler
{
    public abstract class DiscoverCacheContainer
    {
        public abstract DiscoveryCache DiscoveryCache { get; }
    }

    public class GoogleDiscoverCacheContainer : DiscoverCacheContainer
    {
        private IConfiguration _configuration;
        private DiscoveryCache _discoveryCache { get; set; }

        public GoogleDiscoverCacheContainer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override DiscoveryCache DiscoveryCache
        {
            get
            {
                if (_discoveryCache == null)
                {
                    var authority = _configuration["oauth2:google:authority"];
                    var additionalEndpointBaseAddresses = new List<string>();
                    _configuration.GetSection("oauth2:google:additionalEndpointBaseAddresses").Bind(additionalEndpointBaseAddresses);

                    var discoveryClient = new DiscoveryClient(authority);
                    discoveryClient.Policy.ValidateEndpoints = false;
                    foreach (var additionalEndpointBaseAddress in additionalEndpointBaseAddresses)
                    {
                        discoveryClient.Policy.AdditionalEndpointBaseAddresses.Add(additionalEndpointBaseAddress);
                    }
                    _discoveryCache = new DiscoveryCache(discoveryClient);
                }
                return _discoveryCache;
            }
        }
    }

    public class NortonDiscoverCacheContainer : DiscoverCacheContainer
    {
        private IConfiguration _configuration;
        private DiscoveryCache _discoveryCache { get; set; }

        public NortonDiscoverCacheContainer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override DiscoveryCache DiscoveryCache
        {
            get
            {
                if (_discoveryCache == null)
                {
                    var authority = _configuration["oauth2:norton:authority"];
                    var additionalEndpointBaseAddresses = new List<string>();
                    _configuration.GetSection("oauth2:norton:additionalEndpointBaseAddresses").Bind(additionalEndpointBaseAddresses);

                    var discoveryClient = new DiscoveryClient(authority);
                    discoveryClient.Policy.ValidateEndpoints = false;
                    foreach (var additionalEndpointBaseAddress in additionalEndpointBaseAddresses)
                    {
                        discoveryClient.Policy.AdditionalEndpointBaseAddresses.Add(additionalEndpointBaseAddress);
                    }
                    _discoveryCache = new DiscoveryCache(discoveryClient);
                }
                return _discoveryCache;
            }
        }
    }

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
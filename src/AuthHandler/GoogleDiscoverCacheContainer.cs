using System.Collections.Generic;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace AuthHandler
{
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
}
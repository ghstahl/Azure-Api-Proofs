using IdentityModel.Client;

namespace AuthHandler
{
    public abstract class DiscoverCacheContainer
    {
        public abstract DiscoveryCache DiscoveryCache { get; }
    }
}
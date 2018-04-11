using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AuthHandler.Google.ReadAsAsyncCore;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;


namespace AuthHandler.Google
{
    public class ProviderValidator
    {
        private List<RsaSecurityKey> _cachedCerts;
        private DiscoverCacheContainer _discoverCacheContainer;
        private string _audience;
        public DiscoveryResponse _discoveryResponse;
        public ProviderValidator(DiscoverCacheContainer discoverCacheContainer, string audience = null)
        {
            _discoverCacheContainer = discoverCacheContainer;
            _audience = audience;
        }

        async Task<DiscoveryResponse> GetDiscoveryResponseAsync()
        {
            if (_discoveryResponse == null)
            {
                _discoveryResponse = await _discoverCacheContainer.DiscoveryCache.GetAsync();
            }
            return _discoveryResponse;
        }

        public async Task<List<RsaSecurityKey>> FetchCertificates()
        {
            if (_cachedCerts == null)
            {
                var doc = await GetDiscoveryResponseAsync();
                var tokenEndpoint = doc.TokenEndpoint;
                var keys = doc.KeySet.Keys;

                _cachedCerts = new List<RsaSecurityKey>();
                foreach (var webKey in keys)
                {
                    var e = Base64Url.Decode(webKey.E);
                    var n = Base64Url.Decode(webKey.N);

                    var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                    {
                        KeyId = webKey.Kid
                    };

                    _cachedCerts.Add(key);
                }
            }

            return _cachedCerts;
        }
        public async Task<ClaimsPrincipal> ValidateToken(string idToken)
        {
            var doc = await GetDiscoveryResponseAsync();
            var certificates = await this.FetchCertificates();

            TokenValidationParameters tvp = new TokenValidationParameters()
            {
                ValidateActor = false, // check the profile ID

                ValidateAudience = false, // check the client ID
                ValidAudience = null,

                ValidateIssuer = true, // check token came from Google
                ValidIssuers = new List<string> { doc.Issuer },

                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                //     IssuerSigningKeys = certificates.Values.Select(x => new X509SecurityKey(x)),
                IssuerSigningKeys = certificates,

                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromHours(13)
            };

            if (!string.IsNullOrEmpty(_audience))
            {
                tvp.ValidateAudience = true;
                tvp.ValidAudience = _audience;
            }


            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal cp = jsth.ValidateToken(idToken, tvp, out validatedToken);

            return cp;
        }
    }
    public class JwtTokenValidation
    {
        private DiscoveryCache _discoveryCache;
        static Serilog.ILogger _logger = Log.ForContext<JwtTokenValidation>();
        private IConfiguration _configuration;

        public JwtTokenValidation(IConfiguration configuration,
            GoogleDiscoverCacheContainer googleDiscoverCacheContainer,
            NortonDiscoverCacheContainer nortonDiscoverCacheContainer)
        {
            _configuration = configuration;

            // we probably don't want to check for the client_id.  We are only interested in the fact that the authority issued the jwt.
            _nortonProviderValidator =
                new ProviderValidator(nortonDiscoverCacheContainer, null);
            _googleProviderValidator =
                new ProviderValidator(googleDiscoverCacheContainer, null);

        }

        private List<RsaSecurityKey> _cachedCerts;
        private ProviderValidator _nortonProviderValidator;
        private ProviderValidator _googleProviderValidator;

        public async Task<ClaimsPrincipal> ValidateToken(string provider,string idToken)
        {
            ProviderValidator providerValidator = null;
            switch (provider)
            {
                case "google-id_token":
                    providerValidator = _googleProviderValidator;
                    break;
                case "norton-id_token":
                    providerValidator = _nortonProviderValidator;
                    break;
            }

            var cp = await providerValidator.ValidateToken(idToken);
            return cp;
            
        }
    }
}
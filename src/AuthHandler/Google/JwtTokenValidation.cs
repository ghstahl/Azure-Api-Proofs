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
            _nortonDiscoverCacheContainer = nortonDiscoverCacheContainer;
            _googleDiscoverCacheContainer = googleDiscoverCacheContainer;
        }

        private List<RsaSecurityKey> _cachedCerts;
        private NortonDiscoverCacheContainer _nortonDiscoverCacheContainer;
        private GoogleDiscoverCacheContainer _googleDiscoverCacheContainer;


        public async Task<List<RsaSecurityKey>> FetchGoogleCertificates()
        {
            if (_cachedCerts == null)
            {
                var doc = await _googleDiscoverCacheContainer.DiscoveryCache.GetAsync();
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

        private string GoogleClientId => _configuration["Google-ClientId"];
        private string GoogleClientSecret => _configuration["Google-ClientSecret"];
        private string GoogleAuthority => _configuration["oauth2:google:authority"];
       


        public async Task<ClaimsPrincipal> ValidateToken(string idToken)
        {

            var certificates = await this.FetchGoogleCertificates();

            TokenValidationParameters tvp = new TokenValidationParameters()
            {
                ValidateActor = false, // check the profile ID

                ValidateAudience = true, // check the client ID
                ValidAudience = GoogleClientId,

                ValidateIssuer = true, // check token came from Google
                ValidIssuers = new List<string> {"accounts.google.com", "https://accounts.google.com"},

                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                //     IssuerSigningKeys = certificates.Values.Select(x => new X509SecurityKey(x)),
                IssuerSigningKeys = certificates,
              
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromHours(13)
            };

            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal cp = jsth.ValidateToken(idToken, tvp, out validatedToken);

            return cp;
        }
    }
}
using NickBuhro.Bitdefender.Controllers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace NickBuhro.Bitdefender
{
    /// <summary>
    /// Client for BitDefender Control Center API.
    /// </summary>
    public sealed class BitdefenderClient: IDisposable
    {
        private const string DefaultAccessUrl = @"https://cloudgz.gravityzone.bitdefender.com/api";
        private HttpClient _http;

        public Uri AccessUrl => _http.BaseAddress;


        public CompaniesController Companies { get; }
        
        public NetworkController Network { get; }


        public BitdefenderClient(string apikey, string accessurl = null)
        {
            if (apikey == null)
                throw new ArgumentNullException(nameof(apikey));

            var baseAddress = CreateAccessUrl(accessurl);
            var auth = CreateBasicAuthHeaderValue(apikey);

            _http = new HttpClient();
            try
            {
                _http.BaseAddress = baseAddress;
                _http.DefaultRequestHeaders.Authorization = auth;
                _http.DefaultRequestHeaders.Accept.Clear();
                _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                Network = new NetworkController(_http);
                Companies = new CompaniesController(_http);
            }
            catch
            {
                _http.Dispose();
                throw;
            }            
        }

        private static Uri CreateAccessUrl(string accessurl)
        {
            if (string.IsNullOrWhiteSpace(accessurl))
            {
                return new Uri(DefaultAccessUrl);
            }
            return new Uri(accessurl);
        }

        private static AuthenticationHeaderValue CreateBasicAuthHeaderValue(string apikey)
        {
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(apikey + ":"));
            return new AuthenticationHeaderValue("Basic", authHeader);
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}

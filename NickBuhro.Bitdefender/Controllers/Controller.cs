using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NickBuhro.Bitdefender.Controllers
{
    /// <summary>
    /// Base controller class. Can't be inherited from external assemblies.
    /// </summary>
    public abstract class Controller
    {
        private readonly MediaTypeHeaderValue _jsonMediaType = new MediaTypeHeaderValue("application/json");        
        private readonly HttpClient _http;
        private readonly IdGenerator _id;

        public Uri TargetUrl { get; }
        

        internal Controller(HttpClient http, string relativeUri)
        {
            Debug.Assert(http != null);
            Debug.Assert(http.BaseAddress != null);
            Debug.Assert(relativeUri != null);
            Debug.Assert(relativeUri.StartsWith("/"));
            
            TargetUrl = new Uri(http.BaseAddress.AbsoluteUri.TrimEnd('/') + relativeUri);

            _http = http;
            _id = new IdGenerator();
        }

        /// <summary>
        /// Send JSON-RPC 2.0 request to the TargetUrl.
        /// </summary>
        /// <typeparam name="TParams">Parameters type. Could be JToken or other JSON-serializable type.</typeparam>
        /// <typeparam name="TResult">Result type. Should be deserializable by JSON.NET.</typeparam>
        /// <param name="method">JSON-RPC 2.0 method name.</param>
        /// <param name="args">JSON-RPC 2.0 method parameters.</param>
        /// <returns>JSON-RPC 2.0 response result if the responce doesn't contain an error.</returns>
        /// <exception cref="JsonRpcError">Throws if JSON-RPC 2.0 response contains an error.</exception>
        protected async Task<TResult> Send<TParams, TResult>(string method, TParams args, CancellationToken ct)
        {
            var jsonRequest = new JsonRpcRequest<TParams>()
            {
                Id = _id.Next(),
                Method = method,
                Params = args
            };        
            
            var contentString = JsonConvert.SerializeObject(jsonRequest, Formatting.Indented);
            var content = new StringContent(contentString);
            content.Headers.ContentType = _jsonMediaType;

            using (var response = await _http.PostAsync(TargetUrl, content, ct).ConfigureAwait(false))
            {                   
                var s = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonResponse = JsonConvert.DeserializeObject<JsonRpcResponse<TResult>>(s);

                Debug.Assert(jsonRequest.Id == jsonResponse.Id);

                if (jsonResponse.Error != null)
                {
                    throw new JsonRpcException(jsonResponse.Error);
                }
                return jsonResponse.Result;
            }
        }
    }
}

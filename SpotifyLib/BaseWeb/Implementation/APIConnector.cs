using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.BaseWeb.DTO;
using System.Net.Http;

namespace SpotifyLib.BaseWeb.Implementation
{
    public class APIConnector : IAPIConnector
    {
        private IHttpClient _httpClient;
        private IJSONSerializer _jsonSerializer;
        private IAuthenticator _auhtenticator;
        private Uri _baseUri;

        public async Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Post, headers, body);
        }

        private async Task<T> SendApiRequest<T>(Uri uri, HttpMethod httpMethod,
            IDictionary<string, string> headers = null, 
            object body = null, IDictionary<string, string> parameters = null)
        {
            var request = CreateRequest(uri, httpMethod, headers, body, parameters);
            await _auhtenticator.Apply(request, this);
            var apiResponse = await SendSerializedRequest<T>(request).ConfigureAwait(false);
            return apiResponse.Body;
        }

        private async Task<ApiResponse<T>> SendSerializedRequest<T>(Request request)
        {
            _jsonSerializer.SerializeRequest(request);
            var response = await DoRequest(request).ConfigureAwait(false);
            return _jsonSerializer.DeserealizeResponse<T>(response);
        }

        private async Task<Response> DoRequest(Request request)
        {
            var response = await _httpClient.DoRequest(request).ConfigureAwait(false);
            return response;
        }

        private Request CreateRequest(Uri uri, HttpMethod method, 
            IDictionary<string, string> headers, object body, IDictionary<string, string> parameters)
        {
            return new Request
            {
                Body = body,
                Method = method,
                Uri = _baseUri,
                EndPoint = uri,
                Headers = headers ?? new Dictionary<string, string>(),
                Parameters = parameters ?? new Dictionary<string, string>()
            };
        }
    }
}

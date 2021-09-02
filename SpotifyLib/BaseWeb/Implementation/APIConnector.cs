using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.BaseWeb.DTO;

namespace SpotifyLib.BaseWeb.Implementation
{
    public class APIConnector : IAPIConnector
    {
        private IHttpClient _httpClient;
        private IJSONSerializer _jsonSerializer;
        private IAuthenticator _auhtenticator;
        private Uri _baseUri;

        public APIConnector(Uri baseUri, IAuthenticator authenticator, IJSONSerializer jsonSerializer, IHttpClient httpClient)
        {
            _baseUri = baseUri;
            _auhtenticator = authenticator;
            _jsonSerializer = jsonSerializer;
            _httpClient = httpClient;
        }

        public async Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Post, headers, body);
        }

        public async Task<T> Get<T>(Uri uri)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Get);
        }
        public async Task<T> Get<T>(Uri uri, IDictionary<string, string> headers)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Get, headers);
        }

        private async Task<T> SendApiRequest<T>(Uri uri, HttpMethod httpMethod,
            IDictionary<string, string> headers = null, 
            object body = null, IDictionary<string, string> parameters = null)
        {
            var request = CreateRequest(uri, httpMethod, headers, body, parameters);
            await ApplyAuthenticator(request);
            var apiResponse = await SendSerializedRequest<T>(request).ConfigureAwait(false);
            return apiResponse.Body;
        }

        private async Task ApplyAuthenticator(Request request)
        {
            if (!request.EndPoint.IsAbsoluteUri || request.EndPoint.AbsoluteUri.Contains("https://api.spotify.com"))
                await _auhtenticator.Apply(request, this);

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

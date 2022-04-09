using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

using SpotifyLib.Interfaces.BaseWeb;
using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.Interfaces;
using SpotifyLib.Utils;
using SpotifyLib.Exceptions;

namespace SpotifyLib.BaseWebImplementation
{
    public class APIConnector : IAPIConnector
    {
        private IAuthenticator _authenticator;
        private IHttpClient _httpClient;
        private IJSONSerializer _jsonSerializer;
        private IRetryHandler _retryHandler;
        private Uri _baseUri;

        public APIConnector(Uri baseUri, IJSONSerializer jsonSerializer, IHttpClient httpClient, IAuthenticator authenticator=null, bool useCookies=false)
        {
            _baseUri = baseUri;
            _jsonSerializer = jsonSerializer;
            _httpClient = httpClient;
            _authenticator = authenticator;
            _retryHandler = new RetryHandler();
        }

        public async Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Post, headers, body);
        }

        public async Task<T> Post<T>(Uri uri, object body)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Post, body: body);
        }

        public async Task<Response> Post(Uri uri, object body)
        {
            var result = await SendApiRequestFullResponse(uri, HttpMethod.Post, body: body);
            return result;
        }


        public async Task<Response> Put(Uri uri)
        {
            return await SendApiRequestFullResponse(uri, HttpMethod.Put);
        }

        public async Task<T> Get<T>(Uri uri)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Get);
        }
        public async Task<T> Get<T>(Uri uri, IDictionary<string, string> headers)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Get, headers);
        }

        public async Task<T> Get<T>(Uri uri, IDictionary<string, string> headers, IDictionary<string, string> parameters)
        {
            return await SendApiRequest<T>(uri, HttpMethod.Get, headers, parameters: parameters);
        }
        public async Task<Response> Get(Uri uri)
        {
            return await SendApiRequestFullResponse(uri, HttpMethod.Get);
        }

 
        public IEnumerable<Cookie> GetRequestCookies(Uri uri)
        {
            Uri fullUri;
            if (uri.IsAbsoluteUri)
                fullUri = uri;
            else
                fullUri = new(_baseUri, uri);
            return _httpClient.GetCookies(fullUri);
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

        private async Task<Response> SendApiRequestFullResponse(Uri uri, HttpMethod httpMethod, 
            IDictionary<string, string> headers = null, object body = null, 
            IDictionary<string, string> parameters = null)
        {
            var request = CreateRequest(uri, httpMethod, headers, body, parameters);
            await ApplyAuthenticator(request);
            var apiResponse = await SendSerializedRequest<object>(request).ConfigureAwait(false);
            return apiResponse.Response;
        }

        private async Task ApplyAuthenticator(Request request)
        {
            if (!request.Uri.ToString().Contains("token"))
            {
                await _authenticator.ApplyAsync(request, this);
            }

        }

        private async Task<ApiResponse<T>> SendSerializedRequest<T>(Request request)
        {
            if(request.Body is IDictionary<string, string>)
            {
                var httpContent = new FormUrlEncodedContent((IDictionary<string,string>)request.Body);
                httpContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                request.Body = httpContent;
            }
            else
                _jsonSerializer.SerializeRequest(request);
            var response = await DoRequest(request).ConfigureAwait(false);
            return _jsonSerializer.DeserealizeResponse<T>(response);
        }

        private async Task<Response> DoRequest(Request request)
        {
            var response = await _httpClient.DoRequest(request).ConfigureAwait(false);
            if(_retryHandler != null)
            {
                response = await _retryHandler.HandleRetry(request, response, async (newRequest) =>
                {
                    await ApplyAuthenticator(request).ConfigureAwait(false);
                    var newResponse = await _httpClient.DoRequest(request).ConfigureAwait(false);
                    return newResponse;
                }).ConfigureAwait(false);
            }
            if (response.StatusCode != HttpStatusCode.OK)
                throw new SpotifyApiException(response);
            return response;
        }

        private Request CreateRequest(Uri uri, HttpMethod method, 
            IDictionary<string, string> headers, object body, IDictionary<string, string> parameters)
        {
            return new Request
            {
                Body = body,
                Method = method,
                Uri = uri.IsAbsoluteUri ? uri : _baseUri,
                EndPoint = uri.IsAbsoluteUri ? new Uri(string.Empty, UriKind.Relative) : uri,
                Headers = headers ?? new Dictionary<string, string>(),
                Parameters = parameters ?? new Dictionary<string, string>()
            };
        }


        public bool SetAuthenticator(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
            return true;
        }
    }
}

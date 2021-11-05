using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using BaseWeb.DTO;
using BaseWeb.Extensions;
using BaseWeb.Interfaces;

namespace BaseWeb.Implementation
{
    public class NetHttpClient : IHttpClient
    {
        private HttpClient _httpClient;
        private CookieContainer _cookieContainer;

        public NetHttpClient(bool useCookies=false, bool autoRedirect = true)
        {
            var clientHandler = BuildClientHandler(useCookies, autoRedirect);
            if (clientHandler != null)
                _httpClient = new(clientHandler);
            else
                _httpClient = new();
            
        }
        public async Task<Response> DoRequest(Request request)
        {
            var httpRequestMessage = BuildRequestMessage(request);
            var httpResponse = await _httpClient.SendAsync(httpRequestMessage);
            var response = await BuildResponse(httpResponse);
            return response;
        }

        public IEnumerable<Cookie> GetCookies(Uri uri)
        {
            if (_cookieContainer == null)
                return null;

            var result = _cookieContainer.GetCookies(uri).Cast<Cookie>();
            return result;
        }

        private HttpClientHandler BuildClientHandler(bool useCookies, bool autoRedirect)
        {
            if (!useCookies && autoRedirect)
                return null;

            var clientHandler = new HttpClientHandler();

            if (useCookies)
            {
                _cookieContainer = new CookieContainer();
                clientHandler.CookieContainer = _cookieContainer;
            }
            if (!autoRedirect)
                clientHandler.AllowAutoRedirect = autoRedirect;

            return clientHandler;
        }
        private HttpRequestMessage BuildRequestMessage(Request request)
        {
            var fullUri = new Uri(request.Uri, request.EndPoint).ApplyParameters(request.Parameters);
            var requestMsg = new HttpRequestMessage(request.Method, fullUri);
            foreach(var header in request.Headers)
            {
                requestMsg.Headers.Add(header.Key, header.Value);
            }

            switch (request.Body)
            {
                case string body:
                    requestMsg.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    break;
                case HttpContent body:
                    requestMsg.Content = body;
                    break;
            }
            return requestMsg;

        }

        private async Task<Response> BuildResponse(HttpResponseMessage message)
        {
            var body = await message.Content.ReadAsStringAsync();
            var statusCode = message.StatusCode;
            var headers = message.Headers.ToDictionary(header => header.Key, header => header.Value.First());
            var contentType = message.Content.Headers?.ContentType?.MediaType;

            return new Response(headers)
            {
                Uri = message.Headers.Location,
                ContentType = contentType,
                StatusCode = statusCode,
                Body = body
            };

        }
    }
}

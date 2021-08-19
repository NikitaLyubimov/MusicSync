using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.BaseWeb.DTO;
using SpotifyLib.BaseWeb.Extensions;
using SpotifyLib.BaseWeb.Interfaces;

namespace SpotifyLib.BaseWeb.Implementation
{
    public class NetHttpClient : IHttpClient
    {
        private HttpClient _httpClient;

        public NetHttpClient()
        {
            _httpClient = new HttpClient();
        }
        public async Task<Response> DoRequest(Request request)
        {
            var httpRequestMessage = BuildRequestMessage(request);
            var httpResponse = await _httpClient.SendAsync(httpRequestMessage);
            var response = await BuildResponse(httpResponse);

            return response;
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
                ContentType = contentType,
                StatusCode = statusCode,
                Body = body
            };

        }
    }
}

using System;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.Interfaces.BaseWeb;

namespace SpotifyLib.BaseWebImplementation
{
    public class JSONSerializer : IJSONSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JSONSerializer()
        {
            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            };
        }
        public ApiResponse<T> DeserealizeResponse<T>(Response response)
        {
            if(response.ContentType.Equals("application/json", StringComparison.Ordinal) || response.ContentType == null)
            {
                var body = JsonConvert.DeserializeObject<T>(response.Body, _serializerSettings);
                return new ApiResponse<T>(response, body);
            }
            return new ApiResponse<T>(response);
        }

        public void SerializeRequest(Request request)
        {
            if (request.Body is string || request.Body is HttpContent || request.Body is null)
                return;

            request.Body = JsonConvert.SerializeObject(request.Body, _serializerSettings);
            
        }
    }
}

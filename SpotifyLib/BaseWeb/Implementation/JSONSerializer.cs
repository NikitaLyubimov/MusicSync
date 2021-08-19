using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SpotifyLib.BaseWeb.DTO;
using SpotifyLib.BaseWeb.Interfaces;

namespace SpotifyLib.BaseWeb.Implementation
{
    public class JSONSerializer : IJSONSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JSONSerializer()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
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

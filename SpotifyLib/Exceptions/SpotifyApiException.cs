using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyLib.DTO.BaseWeb;

namespace SpotifyLib.Exceptions
{
    public class SpotifyApiException : Exception
    {
        public Response Response { get; set; }

        public SpotifyApiException(Response response) : base(GetSpotifyErrorMessage(response))
        {
            Response = response;
        }

        private static string? GetSpotifyErrorMessage(Response response)
        {
            var body = response.Body as string;
            if (string.IsNullOrEmpty(body))
                return null;

            try
            {
                var bodyObject = JObject.Parse(body);

                var error = bodyObject.Value<JToken>("error");
                if (error == null)
                    return null;
                else if (error.Type == JTokenType.String)
                    return error.ToString();
                else if (error.Type == JTokenType.Object)
                    return error.Value<string>("message");
            }
            catch (JsonReaderException)
            {
                return null;
            }
            return null;
        }
    }
}

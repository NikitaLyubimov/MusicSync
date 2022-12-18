using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace SpotifyLib.DTO.BaseWeb
{
    public class Response
    {
        public Response(IDictionary<string, string> headers)
        {
            Headers = new ReadOnlyDictionary<string, string>(headers);
        }
        public Uri Uri { get; set; }
        public string Body { get; set; }
        public IReadOnlyDictionary<string, string> Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }
    }
}

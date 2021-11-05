using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWeb.DTO
{
    public class ApiResponse<T>
    {
        public ApiResponse(Response response, T body = default)
        {
            Body = body;
            Response = response;
        }
        public T Body { get; }
        public Response Response { get;  }
    }
}

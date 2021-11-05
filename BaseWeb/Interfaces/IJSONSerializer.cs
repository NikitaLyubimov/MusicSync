using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseWeb.DTO;

namespace BaseWeb.Interfaces
{
    public interface IJSONSerializer
    {
        void SerializeRequest(Request request);
        ApiResponse<T> DeserealizeResponse<T>(Response response);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseWeb.DTO;

namespace BaseWeb.Interfaces
{
    public interface IAuthenticator
    {
        Task ApplyAsync(Request request, IAPIConnector apiConnector);
    }
}

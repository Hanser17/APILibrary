using MCP_Server.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Handlers
{
    public interface IMcpHandler
    {
        string Method { get; }

        Task<JsonRpcResponse> HandleAsync(
            JsonRpcRequest request,
            CancellationToken cancellationToken);
    }
}

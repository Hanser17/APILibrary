using MCP_Server.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Handlers
{
    public class InitializeHandler : IMcpHandler
    {
        public string Method => JsonRpcConstants.Initialize;

        public Task<JsonRpcResponse> HandleAsync(
            JsonRpcRequest request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new JsonRpcResponse
                {
                    Id = request.Id,

                    Result = new
                    {
                        protocolVersion = "2025-03-26",

                        capabilities = new
                        {
                            tools = new
                            {
                                listChanged = false
                            }
                        },

                        serverInfo = new
                        {
                            name = "Library_MCP",
                            version = "1.0.0"
                        }
                    }
                });
        }
    }
}

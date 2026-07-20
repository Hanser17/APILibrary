using MCP_Server.MCP_Model;
using MCP_Server.Protocol;
using MCP_Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Handlers
{
    public class ListToolsHandler : IMcpHandler
    {
        private readonly IToolRegistry _registry;
        private readonly ToolMapper _mapper;

        public string Method => JsonRpcConstants.ToolsList;

        public ListToolsHandler(
            IToolRegistry registry,
            ToolMapper mapper)
        {
            _registry = registry;
            _mapper = mapper;
        }

        public Task<JsonRpcResponse> HandleAsync(
            JsonRpcRequest request,
            CancellationToken cancellationToken)
        {
            var tools =
                _mapper.Map(_registry.GetAll());

            return Task.FromResult(
                new JsonRpcResponse
                {
                    Id = request.Id,

                    Result = new
                    {
                        tools
                    }
                });
        }
    }
}

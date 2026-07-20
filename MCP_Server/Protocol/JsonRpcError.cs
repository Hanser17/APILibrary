using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Protocol
{
    public class JsonRpcError
    {
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCP_Server.Protocol
{
    public class JsonRpcRequest
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";


        [JsonPropertyName("id")]
        public object? Id { get; set; }


        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;


        [JsonPropertyName("params")]
        public object? Params { get; set; }
    }
}

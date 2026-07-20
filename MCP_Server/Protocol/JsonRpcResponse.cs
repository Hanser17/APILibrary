using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCP_Server.Protocol
{
    public class JsonRpcResponse
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";


        [JsonPropertyName("id")]
        public object? Id { get; set; }


        [JsonPropertyName("result")]
        public object? Result { get; set; }


        [JsonPropertyName("error")]
        public JsonRpcError? Error { get; set; }
    }
}

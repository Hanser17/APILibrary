using MCP_Server.MCP_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCP_Server.Internal_Models
{
    public class ToolDefinitionSchema
    {
        public string Type { get; set; } = "object";

        public Dictionary<string, object> Properties { get; set; }
            = new();

        public List<string> Required { get; set; }
            = new();

        public bool AdditionalProperties { get; set; }
    }
}

using MCP_Server.Internal_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.MCP_Model
{
    public class ToolMapper
    {
        public McpTool Map(ToolDefinition tool)
        {
            return new McpTool
            {
                Name = tool.Name,
                Title = tool.Name,
                Description = tool.Description,
                InputSchema = MapSchema(tool.Schema)
            };
        }

        private McpSchema MapSchema(ToolDefinitionSchema schema)
        {
            if (schema == null)
            {
                return new McpSchema
                {
                    Type = "object",
                    AdditionalProperties = false
                };
            }

            return new McpSchema
            {
                Type = schema.Type,
                Properties = new Dictionary<string, object>(schema.Properties),
                Required = new List<string>(schema.Required),
                AdditionalProperties = schema.AdditionalProperties
            };
        }
    

        public IEnumerable<McpTool> Map(
            IEnumerable<ToolDefinition> tools)
        {
            return tools.Select(Map);
        }
    }
}

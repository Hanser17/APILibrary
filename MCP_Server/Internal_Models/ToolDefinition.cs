using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Internal_Models
{
    public class ToolDefinition
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string HttpMethod { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public bool RequiresAuthentication { get; set; }

        public ToolDefinitionSchema? Schema { get; set; }
    }
}

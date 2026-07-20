using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Tools
{
    public class ToolFilter
    {
        private readonly HashSet<string> _excludedTools;

        public ToolFilter(IConfiguration configuration)
        {
            var excludedTools = configuration
                .GetSection("McpTools:ExcludedTools")
                .Get<string[]>();

            _excludedTools = excludedTools != null
                ? new HashSet<string>(excludedTools)
                : new HashSet<string>();
        }

        public bool ShouldExpose(string toolName)
        {
            return !_excludedTools.Contains(toolName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Protocol
{
    public class JsonRpcConstants
    {
        public const string Version = "2.0";

        public const string Initialize = "initialize";

        public const string ToolsList = "tools/list";

        public const string ToolsCall = "tools/call";
    }
}

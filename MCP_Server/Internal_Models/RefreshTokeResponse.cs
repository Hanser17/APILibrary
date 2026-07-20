using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCP_Server.Internal_Models
{
    public class RefreshTokeResponse
    {
        public string Token { get; set; }

        public string RefreshTaken { get; set; }

        [JsonIgnore]
        public DateTime? Expires { get; set; }
    }
}

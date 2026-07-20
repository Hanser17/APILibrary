using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_Server.Internal_Models
{
    public class LoginResponse
    {
        public string? JWToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? Expiration { get; set; }
    }
}

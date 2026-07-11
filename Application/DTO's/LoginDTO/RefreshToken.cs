
using System.Text.Json.Serialization;

namespace Application.DTO_s.LoginDTO
{
    public class RefreshToken
    {

        public string Token { get; set; }

        public string RefreshTaken { get; set; }

        [JsonIgnore]
        public DateTime? Expires { get; set; }
        [JsonIgnore]
        public DateTime? Created { get; set; }
    }
}

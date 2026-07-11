

namespace Domain.JWTSettings
{
    public class JWT_Secrets
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }
    }
}

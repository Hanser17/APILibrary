

namespace Application.DTO_s.LoginDTO
{
   
        public class AuthenticationResponse
        {
            public string Id { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string JWToken { get; set; }

            public string RefreshToken { get; set; }

        }

}

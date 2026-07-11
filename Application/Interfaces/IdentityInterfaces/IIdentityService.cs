using Application.DTO_s.LoginDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IdentityInterfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> AuthenticateAsync(Authenticationrequest dtoRequest);

        Task<AuthenticationResponse> GetRefreshTokenAsync(RefreshToken refreshToken);
    }
}

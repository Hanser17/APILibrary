using Application.DTO_s.LoginDTO;
using Application.Interfaces.IdentityInterfaces;
using Identity.InterfacesImplementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IIdentityService _identity;
        public LoginController(IIdentityService identityService)
        {
            _identity = identityService;
        }

        /// <summary>
        /// Devuelv un token de acceso y un refresh token al usuario autenticado.
        /// </summary>
        /// <param name="authentication"></param>
        /// <returns>Token y  refresh Token</returns>
        /// 
        
        [HttpPost("Login-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] Authenticationrequest authentication)
        {
            var response = await _identity.AuthenticateAsync(authentication);
            return response == null ? BadRequest("Error al registrar el usuario") : Ok(response);
        }

        /// <summary>
        /// Endpoint para refrescar el token de acceso utilizando un refresh token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Nuevo Token y nuevo refresh Token</returns>
        /// 
      
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken([FromBody] RefreshToken request)
        {
            var response = await _identity.GetRefreshTokenAsync(request);
            return response == null ? Unauthorized(new { message = "Refresh token inválido o expirado" }) : Ok(response);

        }
    }
}

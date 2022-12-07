using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.payload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApplication.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request! Username and password not null!");
            }
            try
            {
                var response = _unitOfWork.UserInfoRepo.Authenticate(request);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RotateToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _unitOfWork.TokenRepo.RenewToken(refreshToken);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
            return Ok(response);
        }

    }
}

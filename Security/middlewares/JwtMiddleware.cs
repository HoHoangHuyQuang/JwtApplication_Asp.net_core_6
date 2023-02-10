using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtApplication.Security.middlewares
{
    /// <summary>
    /// middleware validate the token
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSetting _appSettings;

        public JwtMiddleware(RequestDelegate next, AppSetting appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async void Invoke(HttpContext context, IUserInfoRepository repository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null) { }

            // to behind middleware
            await _next(context);
        }
        private void JwtValidate(HttpContext context, string token, IUserInfoRepository repository)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);

                context.Items["User"] = repository.FindById(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

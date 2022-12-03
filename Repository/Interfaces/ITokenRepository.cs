using JwtApplication.Data.models;
using System.Security.Claims;

namespace JwtApplication.Repository.Interfaces
{
    public interface ITokenRepository : IBaseRepository<Token, int>
    {

        string GenerateAccessToken(UserInfo user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}

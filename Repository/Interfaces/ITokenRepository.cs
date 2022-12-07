using JwtApplication.Data.models;
using JwtApplication.Security.payload;

namespace JwtApplication.Repository.Interfaces
{
    public interface ITokenRepository : IBaseRepository<RefreshToken, int>
    {
        AuthResponse RenewToken(string refreshToken);
        public void RevokeToken(string token);
    }
}

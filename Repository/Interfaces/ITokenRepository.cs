using JwtApplication.Data.models;

namespace JwtApplication.Repository.Interfaces
{
    public interface ITokenRepository : IBaseRepository<Token, int>
    {
        public void RevokeToken(string token);
    }
}

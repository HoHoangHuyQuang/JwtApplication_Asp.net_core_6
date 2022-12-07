using JwtApplication.Data;
using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.payload;
using JwtApplication.Security.Utils;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Repository.Implement
{
    public class TokenRepository : BaseRepository<RefreshToken, int>, ITokenRepository
    {
        private readonly DatabaseContext _context;
        public TokenRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public void RevokeToken(string refreshToken)
        {
            var token = _dbSet.AsNoTracking().Where(x => x.Token == refreshToken).FirstOrDefault();
            if (token != null)
            {
                if (!token.IsActive)
                {
                    throw new Exception("Invalid token!!!");
                }

                token.Revoked = DateTime.Now;
                _dbSet.Update(token);
                _context.SaveChanges();
            }
            return;
        }
        public AuthResponse RenewToken(string refreshToken)
        {

            var token = _dbSet.AsNoTracking().Where(x => x.Token == refreshToken).FirstOrDefault();

            if (token != null)
            {
                if (!token.IsActive)
                {
                    throw new Exception("Invalid token!!!");
                }
                var user = token.User;
                var newRefreshToken = JwtUtils.GenerateRefreshToken();
                var newAccessToken = JwtUtils.GenerateAccessToken(user);

                RevokeToken(refreshToken);
                _context.Token.Add(new RefreshToken()
                {
                    Token = newRefreshToken,
                    CreatedTime = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddDays(7),
                    User = user,
                    Revoked = null,
                });
                _context.SaveChanges();

                return new AuthResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };

            }
            return null;
        }


    }
}

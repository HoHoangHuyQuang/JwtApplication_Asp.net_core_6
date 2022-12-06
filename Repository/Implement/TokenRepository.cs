using JwtApplication.Data;
using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Repository.Implement
{
    public class TokenRepository : BaseRepository<Token, int>, ITokenRepository
    {
        private readonly DatabaseContext _context;
        public TokenRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public void RevokeToken(string refreshToken)
        {
            var token = _dbSet.AsNoTracking().Where(x => x.RefreshToken == refreshToken).FirstOrDefault();
            if (token != null)
            {
                token.Revoked = DateTime.Now;
                _dbSet.Update(token);
                _context.SaveChanges();
            }
            return;
        }
    }
}

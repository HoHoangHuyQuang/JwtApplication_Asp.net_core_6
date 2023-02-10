using JwtApplication.Data;
using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.payload;
using JwtApplication.Security.Utils;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Repository.Implement
{
    public class UserInfoRepository : BaseRepository<UserInfo, int>, IUserInfoRepository
    {

        private readonly DatabaseContext _context;
        public UserInfoRepository(DatabaseContext context) : base(context)
        {

            _context = context;
        }

        public override async Task<UserInfo> FindById(int id)
        {
            var user = await _dbSet
                                   .Include(u => u.UserRoles)
                                   .ThenInclude(ur => ur.Role)
                                   .Where(u => u.UserId == id)
                                   .FirstOrDefaultAsync();
            return user;
        }
        public bool IsExistByUsername(string username)
        {
            if (username == null) return false;
            var user = _dbSet.SingleOrDefault(e => e.UserName.Equals(username));
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public AuthResponse Authenticate(LoginRequest request)
        {
            var user = _dbSet.FirstOrDefault(x => x.UserName.Equals(request.Username));

            if (user == null || !request.Password.Equals(user.Password))
            {
                throw new Exception("Username or password is incorrect");
            }
            string accessToken = JwtUtils.GenerateAccessToken(user);
            string refreshToken = JwtUtils.GenerateRefreshToken();

            var isUnique = !_context.Token.Any(x => x.Token == refreshToken);
            while (!isUnique)
            {
                refreshToken = JwtUtils.GenerateRefreshToken();
            }

            _context.Token.Add(new RefreshToken()
            {
                Token = refreshToken,
                User = user,
                CreatedTime = DateTime.Now,
                ExpiredTime = DateTime.Now.AddDays(7),
                Revoked = null,
            });
            _context.SaveChanges();

            return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }



    }
}

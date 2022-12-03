using JwtApplication.Data;
using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.payload;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Repository.Implement
{
    public class UserInfoRepository : BaseRepository<UserInfo, int>, IUserInfoRepository
    {
        public UserInfoRepository(DatabaseContext context) : base(context)
        {
        }

        public override async Task<UserInfo> FindById(int id)
        {
            var user = await _dbSet.Include(u => u.UserRoles)
                                   .ThenInclude(ur => ur.Role)
                                   .Where(u => u.UserId == id)
                                   .FirstOrDefaultAsync();
            return user;
        }
        public UserInfo? GetLoginUser(LoginRequest request)
        {
            var user = _dbSet.SingleOrDefault(e => request.Username.Equals(e.UserName) && request.Password.Equals(e.Password));
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

    }
}

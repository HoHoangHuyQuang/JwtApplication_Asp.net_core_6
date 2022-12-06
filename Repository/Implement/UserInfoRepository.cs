﻿using JwtApplication.Data;
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
            var user = await _dbSet.Include(u => u.UserRoles)
                                   .ThenInclude(ur => ur.Role)
                                   .Where(u => u.UserId == id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();
            return user;
        }
        public AuthResponse Authenticate(LoginRequest request)
        {
            var user = _dbSet.AsNoTracking()
                             .SingleOrDefault(x => x.UserName.Equals(request.Username));

            if (user == null || !request.Password.Equals(user.Password))
            {
                throw new Exception("Username or password is incorrect");
            }
            string accessToken = JwtUtils.GenerateAccessToken(user);
            string refreshToken = JwtUtils.GenerateRefreshToken();

            _context.Token.Add(new Token()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CreatedTime = DateTime.Now,
                ExpiredTime = DateTime.Now.AddDays(7),
                Revoked = null,
            });
            _context.SaveChanges();

            return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };

        }

        public bool IsExistByUsername(string username)
        {
            if (username == null) return false;
            var user = _dbSet.AsNoTracking().SingleOrDefault(e => e.UserName.Equals(username));
            if (user != null)
            {
                return true;
            }
            return false;
        }

    }
}

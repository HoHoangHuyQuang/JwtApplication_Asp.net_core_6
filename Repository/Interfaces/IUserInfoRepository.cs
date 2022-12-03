using JwtApplication.Data.models;
using JwtApplication.Security.payload;

namespace JwtApplication.Repository.Interfaces
{
    public interface IUserInfoRepository : IBaseRepository<UserInfo, int>
    {
        public UserInfo? GetLoginUser(LoginRequest request);
        public bool IsExistByUsername(string username);

    }


}

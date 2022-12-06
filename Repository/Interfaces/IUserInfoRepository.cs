using JwtApplication.Data.models;
using JwtApplication.Security.payload;

namespace JwtApplication.Repository.Interfaces
{
    public interface IUserInfoRepository : IBaseRepository<UserInfo, int>
    {
        AuthResponse Authenticate(LoginRequest request);
        public bool IsExistByUsername(string username);

    }


}

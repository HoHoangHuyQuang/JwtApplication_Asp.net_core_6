using JwtApplication.Security.Utils;

namespace JwtApplication.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserInfoRepository UserInfoRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public ITokenRepository TokenRepo { get; }
        public IJwtUtils JwtUtils { get; }
        public Task CommitAsync();
    }
}

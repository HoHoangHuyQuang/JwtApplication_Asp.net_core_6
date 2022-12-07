namespace JwtApplication.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserInfoRepository UserInfoRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public ITokenRepository TokenRepo { get; }
        public Task CommitAsync();
    }
}

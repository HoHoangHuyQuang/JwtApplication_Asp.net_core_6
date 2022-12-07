using JwtApplication.Data;
using JwtApplication.Repository.Interfaces;

namespace JwtApplication.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _iconfig;


        private IRoleRepository _role;
        private IUserInfoRepository _user;
        private ITokenRepository _token;


        public UnitOfWork(DatabaseContext context, IConfiguration iconfig)
        {
            _context = context;
            _iconfig = iconfig;
        }

        public IUserInfoRepository UserInfoRepo
        {
            get
            {
                if (this._user == null)
                {
                    this._user = new UserInfoRepository(_context);
                }
                return _user;
            }
        }

        public IRoleRepository RoleRepo
        {
            get
            {
                if (this._role == null)
                {
                    this._role = new RoleRepository(_context);
                }
                return _role;
            }
        }

        public ITokenRepository TokenRepo
        {
            get
            {
                if (this._token == null)
                {
                    this._token = new TokenRepository(_context);
                }
                return _token;
            }
        }



        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

using JwtApplication.Data;
using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;

namespace JwtApplication.Repository.Implement
{
    public class RoleRepository : BaseRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context) : base(context)
        {
        }
    }
}

using JwtApplication.Data.models;

namespace JwtApplication.Data
{
    public class DbInitializer
    {
        private readonly DatabaseContext _context;

        public DbInitializer(DatabaseContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            if (_context.UserInfos.Any())
            {
                return;
            }
            _context.Database.EnsureCreated();

            var users = new UserInfo[]
            {
                new UserInfo{DisplayName = "User01", UserName= "string_1", Email="", Password="string_1", CreatedDate= DateTime.Now},
                new UserInfo{DisplayName = "User02", UserName= "string_2", Email="", Password="string_1", CreatedDate= DateTime.Now.AddDays(-5)},
                new UserInfo{DisplayName = "User03", UserName= "string_3", Email="", Password="string_1", CreatedDate= DateTime.Now.AddDays(-2)},
                new UserInfo{DisplayName = "User04", UserName= "string_4", Email="", Password="string_1", CreatedDate= DateTime.Now.AddDays(-1)}
            };
            foreach (UserInfo u in users)
            {
                _context.UserInfos.Add(u);
            }
            _context.SaveChanges();


            var roles = new Role[]
            {
                new Role{Name = "ADMIN", Description=""},
                new Role{Name = "MODERATOR", Description=""},
                new Role{Name = "EDITOR", Description=""}
            };
            foreach (Role r in roles)
            {
                _context.Roles.Add(r);
            }
            _context.SaveChanges();
        }
    }
}

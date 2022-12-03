namespace JwtApplication.Data.models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public UserInfo UserInfo { get; set; }
        public Role Role { get; set; }
    }
}

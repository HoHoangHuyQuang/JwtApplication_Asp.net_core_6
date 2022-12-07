using System.Text.Json.Serialization;

namespace JwtApplication.Data.models
{
    public class UserInfo
    {

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public IList<UserRole> UserRoles { get; set; }
        [JsonIgnore]
        public IList<RefreshToken> RefreshTokens { get; set; }

    }
}

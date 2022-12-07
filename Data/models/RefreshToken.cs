using System.Text.Json.Serialization;

namespace JwtApplication.Data.models
{
    public class RefreshToken
    {


        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public UserInfo User { get; set; }
        public string Token { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsRevoked => Revoked != null;
        public bool IsExpired => DateTime.Now >= ExpiredTime;
        public bool IsActive => !IsExpired && !IsRevoked;





    }
}

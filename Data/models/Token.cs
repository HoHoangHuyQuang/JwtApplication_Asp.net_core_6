using System.Text.Json.Serialization;

namespace JwtApplication.Data.models
{
    public class Token
    {


        [JsonIgnore]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredTime;





    }
}

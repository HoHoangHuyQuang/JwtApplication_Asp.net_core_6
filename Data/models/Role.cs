using System.Text.Json.Serialization;

namespace JwtApplication.Data.models
{
    public class Role
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public IList<UserRole> UserRoles { get; set; }
    }
}

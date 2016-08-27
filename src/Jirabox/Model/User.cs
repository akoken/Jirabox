using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class User
    {        
        [JsonProperty("name")]
        public string UserName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}

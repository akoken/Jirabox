using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class AvatarUrls
    {
        [JsonProperty("16x16")]
        public string Size16 { get; set; }

        [JsonProperty("48x48")]
        public string Size48 { get; set; }
    }
}

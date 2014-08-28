using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class Favourite
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("jql")]
        public string JQL { get; set; }
    }
}

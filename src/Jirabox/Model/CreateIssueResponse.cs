using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class CreateIssueResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

    }
}

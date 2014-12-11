using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class CreateIssueType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

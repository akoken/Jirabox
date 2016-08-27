using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class CreateIssueProject
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}

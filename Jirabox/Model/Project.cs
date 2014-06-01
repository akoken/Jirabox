using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class Project 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lead")]
        public User Lead { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("issueTypes")]
        public List<IssueType> IssueTypes { get; set; }
    }
}

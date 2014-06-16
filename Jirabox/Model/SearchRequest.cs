using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class SearchRequest
    {
        [JsonProperty("jql")]
        public string JQL { get; set; }

        [JsonProperty("startAt")]
        public int StartAt { get; set; }

        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        [JsonProperty("fields")]
        public List<string> Fields { get; set; }

        public SearchRequest()
        {
            Fields = new List<string>();
            Expands = new List<string>();
        }
        [JsonProperty("expand")]
        public List<string> Expands { get; set; }
    }
}

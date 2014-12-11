using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class LogWorkRequest
    {
        [JsonProperty("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonProperty("started")]
        public string Started { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}

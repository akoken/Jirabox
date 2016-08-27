
using Newtonsoft.Json;
namespace Jirabox.Model
{
    public class Attachment
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("content")]
        public string Url { get; set; }
    }
}

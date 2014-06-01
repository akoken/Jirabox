
using Newtonsoft.Json;
namespace Jirabox.Model
{
    public class AddCommentRequest
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}

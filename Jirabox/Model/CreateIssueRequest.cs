using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class CreateIssueRequest
    {
        public CreateIssueRequest()
        {
            Fields = new CreateIssueFields();
        }

        [JsonProperty("fields")]
        public CreateIssueFields Fields { get; set; }
    }
}

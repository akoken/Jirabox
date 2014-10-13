using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class Fields
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("reporter")]
        public User Reporter { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("comment")]
        public CommentItem Comment { get; set; }

        [JsonProperty("attachment")]
        public List<Attachment> Attachments { get; set; }
    }
}

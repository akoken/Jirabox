using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class IssueTypeResponse
    {
        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }
    }
}

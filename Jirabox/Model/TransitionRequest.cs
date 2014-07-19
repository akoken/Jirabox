using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class TransitionRequest
    {
        [JsonProperty("transition")]
        public Transition Transition { get; set; }

        [JsonProperty("update")]
        public Update Update { get; set; }       
    }

    public class Update
    {
        [JsonProperty("comment")]
        public List<TransitionComment> Comments { get; set; }
    }
    public class TransitionComment
    {
        [JsonProperty("add")]
        public Add Add { get; set; }
    }

    public class Add
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }    
}

using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class TransitionRequest
    {
        [JsonProperty("transition")]
        public Transition Transition { get; set; }         
    }  
}

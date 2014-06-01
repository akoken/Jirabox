using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class CommentItem
    {
        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }  
    }
}

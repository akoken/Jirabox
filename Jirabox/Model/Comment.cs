using Newtonsoft.Json;
using System;

namespace Jirabox.Model
{
    public class Comment
    {
        [JsonProperty("id")]
        public int CommentId { get; set; }

        [JsonProperty("author")]
        public User Author { get; set; }

        [JsonProperty("body")]
        public string Message { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }
}

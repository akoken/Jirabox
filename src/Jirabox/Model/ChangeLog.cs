using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jirabox.Model
{
    public class ChangeLog
    {
        [JsonProperty("histories")]
        public List<History> Histories { get; set; }

        public ChangeLog()
        {
            Histories = new List<History>();
        }
    }
}

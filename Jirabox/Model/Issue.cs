using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class Issue
    {
        private string m_KeyString;

        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        #region Special key solution
        [JsonProperty("key")]
        public string ProxyKey
        {
            get
            {
                return Key.ToString();
            }
            set
            {
                m_KeyString = value;
            }
        }

        [JsonIgnore]
        public IssueKey Key
        {
            get
            {
                return IssueKey.Parse(m_KeyString);
            }
        }
        #endregion Special key solution

        [JsonProperty("fields")]
        public Fields Fields { get; set; }


        [JsonProperty("changelog")]
        public ChangeLog ChangeLog { get; set; }
    }
}

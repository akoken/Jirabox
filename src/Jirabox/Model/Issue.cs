using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class Issue
    {
        private string mKeyString;


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
                mKeyString = value;
            }
        }

        [JsonIgnore]
        public IssueKey Key
        {
            get
            {
                return IssueKey.Parse(mKeyString);
            }
        }
        #endregion Special key solution     

        [JsonProperty("fields")]
        public Fields Fields { get; set; }

        [JsonProperty("changelog")]
        public ChangeLog ChangeLog { get; set; }
    }  
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

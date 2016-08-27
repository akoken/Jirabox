using Newtonsoft.Json;
using System;

namespace Jirabox.Model
{
    public class History
    {
        private DateTime createDate;
        [JsonProperty("created")]
        public string CreateDate
        {
            get
            {
                return createDate.ToString("dd.MM.yyyy HH:mm");
            }
            set
            {
                var date = value;
                createDate = DateTime.Parse(string.Format("{0} {1}", date.Substring(0, 10), date.Substring(11, 8)));
            }
        }
    }
}

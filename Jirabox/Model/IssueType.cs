using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class IssueType
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("subtask")]
        public bool IsSubtask { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as IssueType;
            if (other == null) return false;
            else return (this.Id == other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}

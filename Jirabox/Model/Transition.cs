
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Jirabox.Model
{
    public class TransitionObject
    {
        [JsonProperty("transitions")]
        public List<Transition> Transitions { get; set; }
    }
    public class Transition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var target = obj as Transition;

            if (target == null) return false;

            if (this.Id == target.Id)
                return true;

            return false;
        }
    }
}

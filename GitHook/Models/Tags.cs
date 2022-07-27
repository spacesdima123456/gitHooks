using Newtonsoft.Json;

namespace GitHook.Models
{
    public class Tags
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "$type")]
        public string Type { get; set; }
    }
}

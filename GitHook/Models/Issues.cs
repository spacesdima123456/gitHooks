using Newtonsoft.Json;

namespace GitHook.Models
{
    public class Issues
    {
        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }
}

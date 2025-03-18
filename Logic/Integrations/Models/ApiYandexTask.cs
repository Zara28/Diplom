using Newtonsoft.Json;
using PIHelperSh.Core.Convertation;

namespace OfficeTime.Logic.Integrations.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class ApiYandexTask : YandexTask
    {
        [JsonProperty("summary")]
        public string Name { get; set; }

        [JsonProperty("priority.display")]
        public string Priority { get; set; }

        [JsonProperty("start")]
        public DateTime? DateStart { get; set; }

        [JsonProperty("end")]
        public DateTime? DateEnd { get; set; }

        [JsonProperty("status.display")]
        public string Status { get; set; }

        [JsonProperty("queue.display")]
        public string Queue { get; set; }

        [JsonProperty("storyPoints")]
        public int? Estimate { get; set; }

        [JsonProperty("epic.display")]
        public string Epic { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("feature")]
        public string Feature { get; set; }
    }
}

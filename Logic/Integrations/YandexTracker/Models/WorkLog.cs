using Newtonsoft.Json;
using PIHelperSh.Core.Convertation;

namespace OfficeTime.Logic.Integrations.YandexTracker.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Worklog
    {
        [JsonProperty("record.display")]
        public string comment { get; set; }
        [JsonProperty("to.duration")]
        public object Value { get; set; }
        [JsonProperty("to.start")]
        public DateTime? LastUpdated { get; set; }
    }
}

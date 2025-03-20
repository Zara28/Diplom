using Newtonsoft.Json;
using PIHelperSh.Core.Convertation;

namespace OfficeTime.Logic.Integrations.YandexTracker.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("display")]
        public string Name { get; set; }
        [JsonProperty("fields.entityStatus")]
        public string EntityStatus { get; set; }
    }
}
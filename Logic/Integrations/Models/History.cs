using Newtonsoft.Json;
using PIHelperSh.Core.Convertation;

namespace OfficeTime.Logic.Integrations.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class History
    {
        [JsonProperty("issue.key")]
        public string Key { get; set; }
        [JsonProperty("updatedBy.display")]
        public string UpdatedBy { get; set; }
        [JsonProperty("worklog")]
        public List<Worklog> Worklogs { get; set; }
    }
}

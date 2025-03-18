using Newtonsoft.Json;

namespace OfficeTime.Logic.Integrations.Models
{
    public class User
    {
        public string Id { get; set; }

        [JsonProperty("display")]
        public string Name { get; set; }
    }
}
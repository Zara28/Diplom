using Goldev.Core.Attributes;

namespace OfficeTime.Logic.Integrations
{
    [AutoConfiguration]
    public class YandexTrackerConfiguration
    {
        public string BaseUrl { get; set; }

        public string Token { get; set; }

        public string OrganizationId { get; set; }
    }
}

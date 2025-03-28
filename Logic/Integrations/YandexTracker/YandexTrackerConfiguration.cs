using Goldev.Core.Attributes;

namespace OfficeTime.Logic.Integrations.YandexTracker
{
    [AutoConfiguration]
    public class YandexTrackerConfiguration
    {
        public string BaseUrl { get; set; }

        public string Token { get; set; }

        public string OrganizationId { get; set; }
    }

    [AutoConfiguration]
    public class Constants
    {
        public string UrlNotification { get; set; }

        public string UrlGenerate { get; set; }

        public string TelegramMain { get; set; }
        public string FIODirector { get; set; }
        public string CompanyName { get; set; }
    }
}

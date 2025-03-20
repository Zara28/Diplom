using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace OfficeTime.Logic.Integrations.YandexTracker
{
    public static class RestSharpExtentions
    {
        public static RestClient CreateClient(this YandexTrackerConfiguration configuration)
        {
            RestClient restClient = new(configuration.BaseUrl, configureSerialization: s => s.UseNewtonsoftJson());
            restClient.AddDefaultHeader("Authorization", $"OAuth {configuration.Token}");
            restClient.AddDefaultHeader("X-Cloud-Org-ID", configuration.OrganizationId);

            return restClient;
        }
    }
}

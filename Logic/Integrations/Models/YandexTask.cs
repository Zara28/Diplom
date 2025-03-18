using AutoMapper;

namespace OfficeTime.Logic.Integrations.Models
{
    [AutoMap(typeof(ApiYandexTask))]
    public class YandexTask
    {
        public string Id { get; set; }

        public string Key { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<string> Tags { get; set; }

        public User Assignee { get; set; }

        public List<User> Followers { get; set; }

        public string Status { get; set; }

        public Project Project { get; set; }

        public string Queue { get; set; }

        public string Spent { get; set; }

        public int? Estimate { get; set; }
        public string Epic { get; set; } = string.Empty;

        public List<History> History { get; set; }

        public string Feature { get; set; }
    }
}

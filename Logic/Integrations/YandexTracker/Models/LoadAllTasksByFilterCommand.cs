using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Integrations.YandexTracker;

namespace OfficeTime.Logic.Integrations.YandexTracker.Models
{
    public class LoadAllTasksByFilterCommand : IRequestModel<List<YandexTask>>
    {
        public DateTime? EndIntervalEnding { get; set; }
        public DateTime? StartIntervalEnding { get; set; }

        public List<string>? Statuses { get; set; }

        public string? UserId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LoadAllTasksByFilterCommand command &&
                   EndIntervalEnding == command.EndIntervalEnding &&
                   StartIntervalEnding == command.StartIntervalEnding;
        }
    }
    public class LoadADataReportCommand : IRequestModel<List<Report>>
    {
        public DateTime? EndIntervalEnding { get; set; }
        public DateTime? StartIntervalEnding { get; set; }

        public override bool Equals(object? obj)
        {
            var command = obj as LoadADataReportCommand;

            return EndIntervalEnding == command.EndIntervalEnding &&
                   StartIntervalEnding == command.StartIntervalEnding;
                               
        }
    }

}

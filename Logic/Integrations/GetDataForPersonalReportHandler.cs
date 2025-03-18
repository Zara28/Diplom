using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using Goldev.Core.Models;
using Humanizer;
using MediatR;
using Microsoft.Extensions.Options;
using OfficeTime.DBModels;
using OfficeTime.Logic.Integrations.Cache;
using OfficeTime.Logic.Integrations.Models;

namespace OfficeTime.Logic.Integrations
{
    public class Report
    {
        public string FIO {  get; set; }
        public double percent { get; set; }
        public double hours { get; set; }
        public double holidays { get; set; }
    }
    public class GetDataForPersonalReportHandler(
        ILogger<GetDataForPersonalReportHandler> logger,
        IMediator mediator,
        MemoryCache<ResponseModel<List<YandexTask>>> cache,
        diplom_adminkaContext context
    ) : AbstractQueryHandler<LoadADataReportCommand, List<Report>>(logger, mediator)
    {
        public override async Task<IHandleResult<List<Report>>> HandleAsync(LoadADataReportCommand query, CancellationToken cancellationToken)
        {
            var cacheResult = cache.GetOrCreate(query, async() => (ResponseModel<List<YandexTask>>)(await mediator.Send(mediator))).Result;
            var holidaydata = context.Holidays.Where(h => h.Datestart >= query.StartIntervalEnding && h.Dateend <= query.EndIntervalEnding);
            var medicaldata = context.Medicals.Where(h => h.Datestart >= query.StartIntervalEnding && h.Dateend <= query.EndIntervalEnding);

            var users = context.Employees;

            List<Report> results = new List<Report>();

            foreach (var user in users)
            {
                var holidays = holidaydata.Where(h => h.Empid == user.Id).OrderBy(h => h.Datestart);
                var medicals = medicaldata.Where(h => h.Empid == user.Id).OrderBy(h => h.Datestart);
                var tracker = cacheResult.Response.Where(r => r.Assignee.Name == user.Yandex);

                double holidaysHours = holidays.Sum(h => SumHour(h.Datestart, h.Dateend));
                double medicalsHours = holidays.Sum(h => SumHour(h.Datestart, h.Dateend));
                double trackerHours = tracker.Sum(t => Convert.ToDouble(t.Spent));
                double totalHours = SumHour(query.StartIntervalEnding, query.EndIntervalEnding);

                var report = new Report()
                {
                    FIO = user.Fio,
                    percent = trackerHours / (totalHours - (holidaysHours + medicalsHours)),
                    holidays = holidaysHours,
                    hours = trackerHours,
                };

                results.Add(report);
            }

            return await Ok(results);
        }

        public double SumHour(DateTime? start, DateTime? end)
        {
            var totalDays = 0;
            for (var date = start; date < end; date = date.Value.AddDays(1))
            {
                if (date.Value.DayOfWeek != DayOfWeek.Saturday
                    && date.Value.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }

            return totalDays*8;
        }
    }
}

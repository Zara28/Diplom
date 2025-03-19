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
using System.Collections.Generic;

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
            var task = async (LoadADataReportCommand query, IMediator mediator) =>
            {
                var result = await mediator.Send(new LoadAllTasksByFilterCommand
                {
                    StartIntervalEnding = query.StartIntervalEnding,
                    EndIntervalEnding = query.EndIntervalEnding
                });

                return result;
            };
            var cacheResult = await cache.GetOrCreate(query, async() => await task(query, mediator));
            var holidaydata = context.Holidays.Where(h => h.Datestart >= query.StartIntervalEnding && h.Dateend <= query.EndIntervalEnding);
            var medicaldata = context.Medicals.Where(h => h.Datestart >= query.StartIntervalEnding && h.Dateend <= query.EndIntervalEnding);

            var users = context.Employees.ToList();

            List<Report> results = new List<Report>();

            foreach (var user in users)
            {
                var holidays = holidaydata.Where(h => h.Empid == user.Id).OrderBy(h => h.Datestart).ToList();
                var medicals = medicaldata.Where(h => h.Empid == user.Id).OrderBy(h => h.Datestart).ToList();
                var data = cacheResult.Response;
                var tracker = data.Where(r => r.Assignee.Name.Trim().ToLower() == user.Yandex?.Trim().ToLower());

                double holidaysHours = holidays.Sum(h => SumHour(h.Datestart, h.Dateend));
                double medicalsHours = holidays.Sum(h => SumHour(h.Datestart, h.Dateend));
                double trackerHours = tracker.Sum(t => ConvertFromString(t.Spent));
                double totalHours = SumHour(query.StartIntervalEnding, query.EndIntervalEnding);
                double percent = trackerHours / (totalHours - (holidaysHours + medicalsHours)) * 100;

                var report = new Report()
                {
                    FIO = user.Fio,
                    percent = percent == double.NaN ? 0 : percent * 100,
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
            for (var date = start; date <= end; date = date.Value.AddDays(1))
            {
                if (date.Value.DayOfWeek != DayOfWeek.Saturday
                    && date.Value.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }

            return totalDays*8;
        }

        public double ConvertFromString(string str)
        {
            TimeSpan ts = new();

            double hours = 0;

            int num = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    num *= 10;
                    num += str[i] - '0';
                }
                else if (num > 0)
                {
                    switch (char.ToLower(str[i]))
                    {
                        case 'm' or 'м':
                            ts += TimeSpan.FromMinutes(num);
                            hours += (double)num / 60;
                            break;
                        case 'h' or 'ч':
                            ts += TimeSpan.FromHours(num);
                            hours += num;
                            break;
                        case 'd' or 'д':
                            ts += TimeSpan.FromDays(num);
                            hours += (double)num * 8;
                            break;
                        case 'w' or 'н':
                            ts += TimeSpan.FromDays(num * 7);
                            hours += (double)num * 8 * 5;
                            break;
                        default:
                            break;
                    }
                    num = 0;
                }
            }
            return hours;
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeTime.Logic.Helpers;
using OfficeTime.Logic.Integrations.YandexTracker;
using OfficeTime.Logic.Integrations.YandexTracker.Models;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OfficeTime.Pages.User
{
    public class IndexModel(IMediator mediator, 
                            ILogger<IndexModel> _logger,
                            IHttpContextAccessor _httpContextAccessor
        ) : PageModel
    {
        public List<HolidayView> Holidays { get; set; }
        public List<MedicalView> Medicals { get; set; }
        public Report Report { get; set; }

        public async Task OnGetAsync()
        {
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var id = _httpContextAccessor.HttpContext.Session.GetId();

            var report = mediator.Send(new LoadADataReportCommand
            {
                StartIntervalEnding = firstDayOfMonth,
                EndIntervalEnding = lastDayOfMonth
            });

            var holiday = mediator.Send(new GetHolidaysQuery
            {
                DateStart = firstDayOfMonth,
                DateEnd = lastDayOfMonth,
                EmpId = id,
            });


            var medicals = mediator.Send(new GetMedicalQuery
            {
                DateStart = firstDayOfMonth,
                DateEnd = lastDayOfMonth,
                EmpId = id,
            });

            //var tasks = new Task[] {holiday, medicals, report};

            //await Task.WhenAll(tasks);

            await medicals;
            await holiday;

            Holidays = holiday.Result.Response;
            Medicals = medicals.Result.Response;
            await report;

            Report = report.Result.Response.FirstOrDefault(r => r.id == id);

        }
    }
}

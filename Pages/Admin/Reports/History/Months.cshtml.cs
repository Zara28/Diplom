using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeTime.Logic.Integrations.YandexTracker.Models;

namespace OfficeTime.Pages.Admin.Reports.History
{
    public class MonthsModel(IMediator mediator) : PageModel
    {
        public class MonthsReport
        {
            public string FIO { get; set; }
            public List<double?> Percents { get; set; }
        }
        public IList<MonthsReport> Model { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int months = 3)
        {
            for (int i = (months - 1)*(-1); i <= 0; i++)
            {
                var data = await mediator.Send(new LoadADataReportCommand
                {
                    StartIntervalEnding = Convert.ToDateTime(DateTime.Now.AddMonths(i)),
                    EndIntervalEnding = Convert.ToDateTime(DateTime.Now.AddMonths(i - 1))
                });

                var list = data.Response;

                if (Model == null)
                {
                    Model = new List<MonthsReport>();
                    list.ForEach(l =>
                    {
                        Model.Add(new MonthsReport
                        {
                            FIO = l.FIO,
                            Percents = new() { l?.percent == double.NaN ? 0 : l.percent },
                        });
                    });
                }
                else
                {
                    list.ForEach(l => Model.Where(m => m.FIO == l.FIO).FirstOrDefault().Percents.Add(l.percent));
                }
            }

            return Page();
        }
    }
}

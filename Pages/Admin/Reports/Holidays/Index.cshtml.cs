using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeTime.Logic.Queries;
using static OfficeTime.Pages.Admin.Reports.Holidays.IndexModel;

namespace OfficeTime.Pages.Admin.Reports.Holidays
{
    public class IndexModel(IMediator mediator) : PageModel
    {
        public int[] CountHoliday { get; set; } = new int[12];
        public List<ChartData> chartData { get; set; }
        public async Task<ActionResult> OnGet()
        {
            var result = await mediator.Send(new GetHolidaysQuery
            {
                DateStart = Convert.ToDateTime("01.01." + DateTime.Now.Year),
                DateEnd = Convert.ToDateTime("31.12." + DateTime.Now.Year),
            });

            var data = result.Response;

            for (int i = 0; i < CountHoliday.Length; i++)
            {
                CountHoliday[i] = data.Where(d => d.Datestart.Value.Month == i - 1).Count();
            }

            chartData = new List<ChartData>
            {
                new ChartData { xValue = "������", yValue = CountHoliday[0] },
                new ChartData { xValue = "�������", yValue = CountHoliday[1] },
                new ChartData { xValue = "����", yValue = CountHoliday[2] },
                new ChartData { xValue = "������", yValue = CountHoliday[3] },
                new ChartData { xValue = "���", yValue = CountHoliday[4] },
                new ChartData { xValue = "����", yValue = CountHoliday[5] },
                new ChartData { xValue = "����", yValue = CountHoliday[6] },
                new ChartData { xValue = "������", yValue = CountHoliday[7] },
                new ChartData { xValue = "��������", yValue = CountHoliday[8] },
                new ChartData { xValue = "�������", yValue = CountHoliday[9] },
                new ChartData { xValue = "������", yValue = CountHoliday[10] },
                new ChartData { xValue = "�������", yValue = CountHoliday[11] },
            };

            ViewData["dataSource"] = chartData;
            return Page();
        }

        public class ChartData
        {
            public string xValue;
            public double yValue;
        }
    }
}

using Goldev.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OfficeTime.Logic.Integrations.YandexTracker;
using OfficeTime.Logic.Integrations.YandexTracker.Cache;
using OfficeTime.Logic.Integrations.YandexTracker.Models;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Reports.Operation
{
    public class IndexModel(IMediator mediator,
        MemoryCache<ResponseModel<List<YandexTask>>> cache) : PageModel
    {
        public IList<Report> Reports { get; set; } = default!;
        public async Task OnGetAsync()
        {
            //await cache.Refresh();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            var start = Convert.ToDateTime(form["start"].ToString());
            var end = Convert.ToDateTime(form["end"].ToString());

            var data = await mediator.Send(new LoadADataReportCommand
            {
                StartIntervalEnding = Convert.ToDateTime(start),
                EndIntervalEnding = Convert.ToDateTime(end)
            });

            Reports = data.Response;

            return Page();
        }
    }
}

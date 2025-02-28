using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Holidays
{
    public class IndexModel(IMediator mediator) : PageModel
    {
        public IList<HolidayView> HolidayView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await mediator.Send(new GetHolidaysQuery());
            HolidayView = result.Response;
        }
        public async Task<IActionResult> OnPostAgree(int holidayId, bool isadmin)
        {
            await mediator.Send(new ApproveHolidayCommand
            {
                Id = holidayId,
                IsLead = !isadmin,
                Value = true
            });

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDisgree(int holidayId, bool isadmin)
        {
            await mediator.Send(new ApproveHolidayCommand
            {
                Id = holidayId,
                IsLead = !isadmin,
                Value = false
            });

            return RedirectToPage("./Index");
        }
    }
}

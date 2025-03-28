using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Goldev.Core.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.GenerationModels;
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
            HolidayView = result.Response.Where(h => h.Canceled != true).ToList();
        }
        public async Task<IActionResult> OnPostAgree(IFormCollection form)
        {
            var holidayId = Convert.ToInt32(form["holidayId"].ToString());
            var isadmin = Convert.ToBoolean(form["isadmin"].ToString());
            await mediator.Send(new ApproveHolidayCommand
            {
                Id = holidayId,
                IsLead = !isadmin,
                Value = true
            });

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDisgree(IFormCollection form)
        {
            var holidayId = Convert.ToInt32(form["holidayId"].ToString());
            var isadmin = Convert.ToBoolean(form["isadmin"].ToString());
            await mediator.Send(new ApproveHolidayCommand
            {
                Id = holidayId,
                IsLead = !isadmin,
                Value = false
            });

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostCreateReport()
        {
            await mediator.Send(new GenerateHolidayCommand
            {
                Year = false
            });
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostCreateYear()
        {
            await mediator.Send(new GenerateHolidayCommand
            {
                Year = true
            });
            return RedirectToPage("./Index");
        }

    }
}

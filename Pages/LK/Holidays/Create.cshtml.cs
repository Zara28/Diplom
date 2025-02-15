using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Helpers;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Holidays
{
    public class CreateModel(IMediator mediator,
                             IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HolidayView HolidayView { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetId();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new CreateHolidayCommand
            {
                Datestart = HolidayView.Datestart,
                Dateend = HolidayView.Dateend,
                Pay = HolidayView.Pay,
                Empid = id
            });

            return RedirectToPage("./Index");
        }
    }
}

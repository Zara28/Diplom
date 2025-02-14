using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeTime.Pages.Admin.Holidays
{
    public class EditLKModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public HolidayView HolidayView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await mediator.Send(new GetHolidaysQuery
            {
                Id = id
            });

            var holidayview = result.Response.FirstOrDefault();

            if (holidayview == null)
            {
                return NotFound();
            }
            HolidayView = holidayview;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new UpdateHolidayCommand
            {
                Id = HolidayView.Id,
                Datestart = HolidayView.Datestart,
                Dateend = HolidayView.Dateend,

                Pay = HolidayView.Pay,

                Isleadapp = HolidayView.Isleadapp,

                Isdirectorapp = HolidayView.Isdirectorapp,

                Dateapp = HolidayView.Dateapp,

                Empid = 0 //todo: Session
            });

            return RedirectToPage("./Index");
        }
    }
}

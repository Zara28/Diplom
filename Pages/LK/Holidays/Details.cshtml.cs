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
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Holidays
{
    public class DetailsLKModel(IMediator mediator) : PageModel
    {
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
            else
            {
                HolidayView = holidayview;
            }
            return Page();
        }
    }
}

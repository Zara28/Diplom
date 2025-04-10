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
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        //public async Task OnGetAsync()
        //{
        //    var result = await mediator.Send(new GetHolidaysQuery());
        //    
        //}

        public async Task OnGetAsync(DateTime? datestart, DateTime? dateEnd)
        {
            var result = await mediator.Send(new GetHolidaysQuery());
            if (datestart.HasValue && dateEnd.HasValue)
            {
                HolidayView = result.Response.Where(h => h.Canceled != true && h.Datestart?.ToDateTime(new TimeOnly()) > datestart && h.Datestart?.ToDateTime(new TimeOnly()) < dateEnd).ToList();
            }
            else
            {
                HolidayView = result.Response.Where(h => h.Canceled != true).ToList();
            }
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
                Type = TypeEnum.PutHolidays
            });
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostCreateYear()
        {
            await mediator.Send(new GenerateHolidayCommand
            {
                Type = TypeEnum.HolidaysT7
            });
            return RedirectToPage("./Index");
        }

    }
}

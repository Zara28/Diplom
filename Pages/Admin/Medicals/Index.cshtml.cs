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

namespace OfficeTime.Pages.Admin.Medicals
{
    public class IndexModel(IMediator mediator) : PageModel
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public IList<MedicalView> MedicalView { get;set; } = default!;

        public async Task OnGetAsync(DateTime? datestart, DateTime? dateEnd)
        {
            var result = await mediator.Send(new GetMedicalQuery());
            if (datestart.HasValue && dateEnd.HasValue)
            {
                MedicalView = result.Response.Where(h => h.Datestart?.ToDateTime(new TimeOnly()) > datestart && h.Datestart?.ToDateTime(new TimeOnly()) < dateEnd).ToList();
            }
            else
            {
                MedicalView = result.Response;
            }
        }
    }
}

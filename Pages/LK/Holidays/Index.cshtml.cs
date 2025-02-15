using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OfficeTime.DBModels;
using OfficeTime.Logic.Helpers;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Holidays
{
    public class IndexLKModel(IMediator mediator,
                              IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public IList<HolidayView> HolidayView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var id = _httpContextAccessor.HttpContext.Session.GetId();
            var result = await mediator.Send(new GetHolidaysQuery
            {
                EmpId = id
            });
            HolidayView = result.Response;
        }
    }
}

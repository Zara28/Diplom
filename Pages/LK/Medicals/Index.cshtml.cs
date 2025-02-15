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
using OfficeTime.Logic.Helpers;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Medicals
{
    public class IndexLKModel(IMediator mediator,
                              IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public IList<MedicalView> MedicalView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var id = _httpContextAccessor.HttpContext.Session.GetId();
            var result = await mediator.Send(new GetMedicalQuery
            {
                EmpId = id
            });
            MedicalView = result.Response;
        }
    }
}

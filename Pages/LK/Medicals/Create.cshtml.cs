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

namespace OfficeTime.Pages.Admin.Medicals
{
    public class CreateModel(IMediator mediator,
                             IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MedicalView MedicalView { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = _httpContextAccessor.HttpContext.Session.GetId();

            await mediator.Send(new CreateMedicalCommand
            {
                DateStart = MedicalView.Datestart.Value.ToDateTime(new TimeOnly()),
                DateEnd = MedicalView.Dateend.Value.ToDateTime(new TimeOnly()),
                EmpId = (int)id
            });

            return RedirectToPage("./Index");
        }
    }
}

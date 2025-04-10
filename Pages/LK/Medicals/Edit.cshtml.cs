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

namespace OfficeTime.Pages.Admin.Medicals
{
    public class EditModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public MedicalView MedicalView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await mediator.Send(new GetMedicalQuery
            {
                Id = id
            });

            var medicalview = result.Response.FirstOrDefault();
            if (medicalview == null)
            {
                return NotFound();
            }
            MedicalView = medicalview;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new UpdateMedicalCommand
            {
                Id = MedicalView.Id,
                Datestart = MedicalView.Datestart.Value.ToDateTime(new TimeOnly()),
                Dateend = MedicalView.Dateend.Value.ToDateTime(new TimeOnly())
            });

            return RedirectToPage("./Index");
        }
    }
}

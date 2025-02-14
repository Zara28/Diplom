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
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Medicals
{
    public class DeleteModel(IMediator mediator) : PageModel
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
            else
            {
                MedicalView = medicalview;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await mediator.Send(new DeleteMedicalCommand
            {
                Id = id
            });

            return RedirectToPage("./Index");
        }
    }
}

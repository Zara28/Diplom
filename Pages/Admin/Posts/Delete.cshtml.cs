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

namespace OfficeTime.Pages.Admin.Posts
{
    public class DeleteModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public PostView PostView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await mediator.Send(new GetPostQuery
            {
                Id = id
            });

            var postview = result.Response.FirstOrDefault();

            if (postview == null)
            {
                return NotFound();
            }
            else
            {
                PostView = postview;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await mediator.Send(new DeletePostCommand
            {
                Id = id.Value
            });

            return RedirectToPage("./Index");
        }
    }
}

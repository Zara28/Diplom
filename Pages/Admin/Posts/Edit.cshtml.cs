using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Posts
{
    public class EditModel(IMediator mediator) : PageModel
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
                Id = id,
            });

            var postview =  result.Response.FirstOrDefault();
            if (postview == null)
            {
                return NotFound();
            }
            PostView = postview;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new UpdatePostCommand
            {
                Id = PostView.Id,
                Name = PostView.Name,
                Rate = PostView.Rate,
            });

            return RedirectToPage("./Index");
        }
    }
}

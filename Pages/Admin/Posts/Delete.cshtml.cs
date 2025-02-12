using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Posts
{
    public class DeleteModel : PageModel
    {
        private readonly OfficeTime.DBModels.diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(OfficeTime.DBModels.diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public PostView PostView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postview = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);

            if (postview == null)
            {
                return NotFound();
            }
            else
            {
                PostView = _mapper.Map<PostView>(postview);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postview = await _context.Posts.FindAsync(id);
            if (postview != null)
            {
                _context.Posts.Remove(postview);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

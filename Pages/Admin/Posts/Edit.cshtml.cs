using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Posts
{
    public class EditModel : PageModel
    {
        private readonly OfficeTime.DBModels.diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public EditModel(OfficeTime.DBModels.diplom_adminkaContext context, IMapper mapper)
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

            var postview =  await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (postview == null)
            {
                return NotFound();
            }
            PostView = _mapper.Map<PostView>(postview);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PostView).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostViewExists(PostView.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostViewExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}

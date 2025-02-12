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

namespace OfficeTime.Pages.Admin.Holidays
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
        public HolidayView HolidayView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidayview = await _context.Holidays.FirstOrDefaultAsync(m => m.Id == id);

            if (holidayview == null)
            {
                return NotFound();
            }
            else
            {
                HolidayView = _mapper.Map<HolidayView>(holidayview);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidayview = await _context.Holidays.FindAsync(id);
            if (holidayview != null)
            {
                _context.Holidays.Remove(holidayview);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

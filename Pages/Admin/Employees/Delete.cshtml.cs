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

namespace OfficeTime.Pages.Admin.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public EmployeeView Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                Employee = _mapper.Map<EmployeeView>(employee);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                Employee = _mapper.Map<EmployeeView>(employee);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeTime.DBModels;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Employees
{
    public class CreateModel : PageModel
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public CreateModel(diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EmployeeView Employee { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Employees.Add(_mapper.Map<DBModels.Employee>(Employee));
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

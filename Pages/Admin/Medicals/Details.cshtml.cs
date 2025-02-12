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

namespace OfficeTime.Pages.Admin.Medicals
{
    public class DetailsModel : PageModel
    {
        private readonly OfficeTime.DBModels.diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public DetailsModel(OfficeTime.DBModels.diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MedicalView MedicalView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalview = await _context.Medicals.FirstOrDefaultAsync(m => m.Id == id);
            if (medicalview == null)
            {
                return NotFound();
            }
            else
            {
                MedicalView = _mapper.Map<MedicalView>(medicalview);
            }
            return Page();
        }
    }
}

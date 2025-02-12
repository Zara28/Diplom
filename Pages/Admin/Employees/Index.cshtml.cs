using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Employees
{
    public class IndexModel : PageModel
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public IndexModel(diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<EmployeeView> Employee { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees.Select(s => _mapper.Map<EmployeeView>(s)).ToListAsync();
        }
    }
}

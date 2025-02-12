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
    public class IndexModel : PageModel
    {
        private readonly OfficeTime.DBModels.diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        public IndexModel(OfficeTime.DBModels.diplom_adminkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<PostView> PostView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PostView = await _context.Posts.Select(p => _mapper.Map<PostView>(p)).ToListAsync();
        }
    }
}

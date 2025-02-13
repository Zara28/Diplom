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
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Posts
{
    public class IndexModel(IMediator mediator) : PageModel
    {
        public IList<PostView> PostView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await mediator.Send(new GetPostQuery());
            PostView = result.Response;
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Employees
{
    public class IndexModel(IMediator mediator) : PageModel
    {

        public IList<EmployeeView> Employee { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await mediator.Send(new GetEmployeesQuery());
            Employee = result.Response;
        }
    }
}

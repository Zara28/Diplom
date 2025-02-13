using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeTime.Pages.Admin.Employees
{
    public class EditModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public EmployeeView Employee { get; set; } = default!;
        [BindProperty]
        public List<PostView> ListPosts { get; set; }
        public SelectList Posts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await mediator.Send(new GetEmployeesQuery
            {
                Id = id,
            });

            var employee = result.Response.FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            Employee = employee;

            var resultPost = await mediator.Send(new GetPostQuery());
            ListPosts = resultPost.Response;
            Posts = new SelectList(ListPosts, nameof(PostView.Id), nameof(PostView.Name));
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

            await mediator.Send(new UpdateEmployeeCommand
            {
                Id = Employee.Id,
                Fio = Employee.Fio,
                Telegram = Employee.Telegram,
                Yandex = Employee.Yandex,
                Datebirth = Employee.Datebirth,
                Datestart = Employee.Datestart,
                Password = Employee.Password,
                PostId = Convert.ToInt32(Employee.Post)
            });

            return RedirectToPage("./Index");
        }
    }
}

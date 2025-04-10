using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeTime.Pages.Admin.Employees
{
    public class EditLKModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public EmployeeView Employee { get; set; } = default!;

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
            return Page();
        }

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
                Datebirth = Employee.Datebirth?.ToDateTime(new TimeOnly()),
                Datestart = Employee.Datestart?.ToDateTime(new TimeOnly()),
                Password = Convert.ToBase64String(Employee.Password.ToSHA256())
            });

            return RedirectToPage("./Details");
        }
    }
}

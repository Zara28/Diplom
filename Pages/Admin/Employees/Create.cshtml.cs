using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Queries;
using OfficeTime.Mapper;
using OfficeTime.ViewModels;
using Syncfusion.EJ2.Navigations;

namespace OfficeTime.Pages.Admin.Employees
{
    public class CreateModel(IMediator mediator) : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            var resultPost = await mediator.Send(new GetPostQuery());
            ListPosts = resultPost.Response;
            Posts = new SelectList(ListPosts, nameof(PostView.Id), nameof(PostView.Name));
            Roles = (Enum.GetValues(typeof(RoleAccess)).Cast<RoleAccess>().Select(
                e => new SelectListItem() { Text = e.ToString(), Value = e.ToString() })).ToList();
            return Page();
        }

        [BindProperty]
        public EmployeeView Employee { get; set; } = default!;
        public List<PostView> ListPosts { get; set; }
        public SelectList Posts {  get; set; }
        public List<SelectListItem> Roles {  get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await mediator.Send(new CreateEmployeeCommand
            {
                Id = Employee.Id,
                Fio = Employee.Fio,
                Telegram = Employee.Telegram,
                Yandex = Employee.Yandex,
                Datebirth = Employee.Datebirth?.ToDateTime(new TimeOnly()),
                Datestart = Employee.Datestart?.ToDateTime(new TimeOnly()),
                Password = Employee.Password,
                PostId = Convert.ToInt32(Employee.Post),
                RoleId = Convert.ToInt32(Employee.Role)
            });

            return RedirectToPage("./Index");
        }
    }
}

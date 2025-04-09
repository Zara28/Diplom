using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using OfficeTime.Logic.Queries;
using OfficeTime.Mapper;

namespace OfficeTime.Pages
{
    public class LoginModel(IMediator mediator,
                            IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public static RoleAccess Role { get; set; } = RoleAccess.NONE;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            var login = form["login"].ToString();
            var password = form["password"].ToString();

            var result = await mediator.Send(new GetEmployeesQuery
            {
                Name = login,
                Password = Convert.ToBase64String(password.ToSHA256())
            });

            var emp = result.Response.FirstOrDefault();

            if(emp == null)
            {
                return NotFound();
            }

            _httpContextAccessor.HttpContext.Session.SetString("session", emp.Id.ToString());
            Role = emp.Role;

            return RedirectToPage("User/Index");
        }
    }
}

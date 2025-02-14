using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeTime.Logic.Queries;

namespace OfficeTime.Pages
{
    public class LoginModel(IMediator mediator,
                            IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string login, string password)
        {
            var result = await mediator.Send(new GetEmployeesQuery
            {
                Name = login,
                Password = password
            });

            var emp = result.Response.FirstOrDefault();

            if(emp == null)
            {
                return NotFound();
            }

            _httpContextAccessor.HttpContext.Session.SetString("session", emp.Id.ToString());

            return RedirectToPage("User/Index");
        }
    }
}

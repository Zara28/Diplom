﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Helpers;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Pages.Admin.Employees
{
    public class DetailsLKModel(IMediator mediator,
                                IHttpContextAccessor _httpContextAccessor) : PageModel
    {
        public EmployeeView Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            int? id = _httpContextAccessor.HttpContext.Session.GetId();

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
            else
            {
                Employee = employee;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection from)
        {
            var date = Convert.ToDateTime(from["date"].ToString());
            var id = _httpContextAccessor.HttpContext.Session.GetId();

            await mediator.Send(new DismissalCommand
            {
                Id = (int)id,
                Date = date,
            });

            return RedirectToPage("./Details");
        }

        public async Task<IActionResult> OnPostCancelAsync(IFormCollection from)
        {
            var id = _httpContextAccessor.HttpContext.Session.GetId();

            await mediator.Send(new DismissalCancelCommand
            {
                Id = (int)id
            });

            return RedirectToPage("./Details");
        }
    }
}

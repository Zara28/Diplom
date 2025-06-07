using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using System.Net;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class UpdateEmployeeCommandHandler : AbstractCommandHandler<UpdateEmployeeCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public UpdateEmployeeCommandHandler(
                ILogger<UpdateEmployeeCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var post = _context.Employees.Where(e => e.Id == command.Id).Select(e => e.Postid).FirstOrDefault();
                var role = _context.Roles.Where(e => e.Id == command.RoleId).Select(e => e.Id).FirstOrDefault();

                var employee = new Employee
                {
                    Id = command.Id.Value,
                    Fio = command.Fio,
                    Telegram = command.Telegram,
                    Yandex = command.Yandex,
                    Datebirth = command.Datebirth,
                    Datestart = command.Datestart,
                    Password = command.Password,
                    Postid = command.PostId ?? post,
                    Accessid = command.RoleId ?? role
                };

                try
                {
                    //_context.Attach(employee).State = EntityState.Modified;
                   _context.Employees.Update(employee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
                    }
                    else
                    {
                        throw;
                    }
                }


                return await Ok();
            }
            catch (Exception e)
            {
                return await BadRequest(e.Message);
            }
            
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}

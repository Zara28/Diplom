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
            var employee = new Employee
            {
                Id = command.Id,
                Fio = command.Fio,
                Telegram = command.Telegram,
                Yandex = command.Yandex,
                Datebirth = command.Datebirth,
                Datestart = command.Datestart,
                Password = command.Password,
                Postid = command.PostId
            };

            _context.Attach(employee).State = EntityState.Modified;

            try
            {
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

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}

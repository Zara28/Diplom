using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.ViewModels;
using System.Net;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class DeleteEmployeeCommandHandler : AbstractCommandHandler<DeleteEmployeeCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public DeleteEmployeeCommandHandler(
                ILogger<DeleteEmployeeCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            var employee = await _context.Employees.FindAsync(command.Id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

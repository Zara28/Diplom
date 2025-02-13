using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using System.Net;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class CreateEmployeeCommandHandler : AbstractCommandHandler<CreateEmployeeCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public CreateEmployeeCommandHandler(
                ILogger<CreateEmployeeCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken = default)
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

            _context.Employees.Add(employee);

            return await Ok();
        }
    }
}

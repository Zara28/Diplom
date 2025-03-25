using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Newtonsoft.Json;
using OfficeTime.DBModels;
using OfficeTime.GenerationModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;
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

                var diss = _context.Dismissals.First(d => d.Empid == command.Id);

                var model = new Dissmiss
                {
                    NameComppany = "Малое предприятие",
                    DateCreate = DateTime.Now,
                    Date = (DateTime)diss.Date,
                    FIO = employee.Fio,
                    Post = _context.Posts.FirstOrDefault(p => p.Id == employee.Postid).Name,
                    DateReport = diss.Datecreate.ToString(),
                    FIODirector = ""
                };

                await _mediator.Send(new DocumentSendCommand
                {
                    InputModel = new Integrations.Refit.Intefaces.InputModel
                    {
                        TypeEnum = TypeEnum.Dissmiss,
                        Payload = (Newtonsoft.Json.Linq.JObject)JsonConvert.SerializeObject(model)
                    }
                });

                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

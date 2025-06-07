using AutoMapper;
using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeTime.DBModels;
using OfficeTime.GenerationModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;
using OfficeTime.Logic.Integrations.Refit.Intefaces;
using OfficeTime.ViewModels;
using System.Net;

namespace OfficeTime.Logic.Handlers.Employees
{
    [TrackedType]
    public class DeleteEmployeeCommandHandler : AbstractCommandHandler<DeleteEmployeeCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;

        [Constant(BlockName = "Constants")]
        private static string _telegramMain;
        [Constant(BlockName = "Constants")]
        private static string _fIODirector;
        [Constant(BlockName = "Constants")]
        private static string _companyName;

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

                var diss = _context.Dismissals.FirstOrDefault(d => d.Empid == command.Id);

                var model = new Dissmiss
                {
                    NameComppany = _companyName,
                    DateCreate = DateTime.Now,
                    Date = (DateTime)(diss == null ? DateTime.Now : diss.Date),
                    FIO = employee.Fio,
                    Post = _context.Posts.FirstOrDefault(p => p.Id == employee.Postid).Name,
                    DateReport = (diss == null ? DateTime.Now : diss.Date).ToString(),
                    FIODirector = _fIODirector
                };

                var taskDoc = Task.Run(async() => await _mediator.Send(new DocumentSendCommand
                {
                    InputModel = new Integrations.Refit.Intefaces.InputModel
                    {
                        TypeEnum = TypeEnum.Dissmiss,
                        Payload = JsonConvert.SerializeObject(model),
                        TelegramId = _telegramMain
                    }
                }));

                var taskTel = Task.Run(async () => await _mediator.Send(new TelegramMessage
                {
                    ChatId = Convert.ToInt64(employee.Telegram),
                    Message = "Вы были удалены из системы"
                }));

                await Task.WhenAll(new Task[] { taskDoc,  taskTel });

                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

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
using System.Net;

namespace OfficeTime.Logic.Handlers.Employees
{
    [TrackedType]
    public class CreateEmployeeCommandHandler : AbstractCommandHandler<CreateEmployeeCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        [Constant(BlockName ="Constants")]
        private static string _telegramMain;
        [Constant(BlockName = "Constants")]
        private static string _fIODirector;
        [Constant(BlockName = "Constants")]
        private static string _companyName;

        public CreateEmployeeCommandHandler(
                ILogger<CreateEmployeeCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
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
                Postid = command.PostId,
                Accessid = command.RoleId
            };

            _context.Employees.Add(employee);

            _context.SaveChanges();

            var post = _context.Posts.FirstOrDefault(p => p.Id == command.PostId);

            //todo убрать в константы
            var model = new AddEmployee
            {
                FIO = command.Fio,
                NameCompany = _companyName,
                Post = post.Name,
                Cost = post.Rate,
                FIODirector = _fIODirector
            };

            await _mediator.Send(new DocumentSendCommand
            {
                InputModel = new Integrations.Refit.Intefaces.InputModel
                {
                    TypeEnum = TypeEnum.AddEmployee,
                    Payload = JsonConvert.SerializeObject(model),
                    TelegramId = _telegramMain,
                }
            });

            await _mediator.Send(new TelegramMessage
            {
                ChatId = Convert.ToInt64(employee.Telegram),
                Message = "В отдел кадров был отправлен приказ о вашем приеме на работу"
            });

            return await Ok();
        }
    }
}

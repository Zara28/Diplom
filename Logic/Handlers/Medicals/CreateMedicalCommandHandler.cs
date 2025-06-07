using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Holidays;
using OfficeTime.Logic.Integrations.Refit.Commands;

namespace OfficeTime.Logic.Handlers.Medicals
{
    public class CreateMedicalCommandHandler : AbstractCommandHandler<CreateMedicalCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CreateMedicalCommandHandler(
                ILogger<CreateMedicalCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public override async Task<IHandleResult> HandleAsync(CreateMedicalCommand command, CancellationToken cancellationToken = default)
        {
            var holidays = _context.Holidays.Where(h => h.Empid == command.EmpId).ToList();

            if (holidays.Any(h => h.Datestart >= command.DateStart && h.Dateend <= command.DateEnd))
            {
                var task = Task.Run(async () => await _mediator.Send(new NotificationSendCommand
                {
                    Message = $"Ваш больничный совпал с отпуском. Обсудите перенос отпуска за эти дни с руководством",
                    Telegram = _context.Employees.FirstOrDefault(f => f.Id == command.EmpId).Telegram
                }));

                await Task.WhenAll(task);
                await BadRequest("Ваш больничный совпал с отпуском. Обсудите перенос отпуска за эти дни с руководством");
            }

            var medical = new Medical
            {
                Datestart = command.DateStart,

                Dateend = command.DateEnd,

                Datecreate = DateTime.Now,

                Empid = command.EmpId
            };

            _context.Medicals.Add(medical);

            _context.SaveChanges();

            return await Ok();
        }
    }
}

using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;

namespace OfficeTime.Logic.Handlers.Employees
{
    [TrackedType]
    public class DismissalCommandHandler : AbstractCommandHandler<DismissalCommand>
    {
        private readonly diplom_adminkaContext _context;

        [Constant(BlockName = "Constants")]
        private static string _telegramMain;
        private IMediator _mediator;

        public DismissalCommandHandler( ILogger<DeleteEmployeeCommandHandler> logger,
                                        IMediator mediator,
                                        diplom_adminkaContext context) : base(logger, mediator)
        {
            _mediator = mediator;
            _context = context;
        }

        public override async Task<IHandleResult> HandleAsync(DismissalCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Dismissals.AddAsync(new Dismissal
                {
                    Empid = command.Id,
                    Date = command.Date,
                    Datecreate = DateTime.Now
                });

                _context.SaveChanges();

                var emp = _context.Employees.FirstOrDefault(e => e.Id == command.Id);

                await _mediator.Send(new NotificationSendCommand
                {
                    Telegram = _telegramMain,
                    Message = $"{emp.Fio} подал заявку на увольнение с {command.Date}"
                });

                return await Ok();
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }
    }
}

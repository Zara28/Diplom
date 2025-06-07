using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class ApproveHolidayCommandHandler : AbstractCommandHandler<ApproveHolidayCommand>
    {
        private readonly diplom_adminkaContext _context;

        public ApproveHolidayCommandHandler(
                ILogger<CreateHolidayCommandHandler> logger,
                IMediator mediator,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
        }

        public override async Task<IHandleResult> HandleAsync(ApproveHolidayCommand command, CancellationToken cancellationToken = default)
        {
            var holiday = _context.Holidays.FirstOrDefault(h => h.Id == command.Id);

            if(!command.Value)
            {
                holiday.Canceled = true;
            }
            else
            {
                if (command.IsLead)
                {
                    holiday.Isleadapp = command.Value;
                }
                else
                {
                    holiday.Isdirectorapp = command.Value;
                    holiday.Dateapp = DateTime.Now;
                }
            }
            _context.Holidays.Update(holiday);
            _context.SaveChanges();

            var emp = _context.Employees.FirstOrDefault(e => e.Id == holiday.Empid);

            var who = command.IsLead ? "Лид" : "Руководитель";
            var what = command.Value ? "одобрил" : "отклонил";

            var task = Task.Run(async() => await _mediator.Send(new NotificationSendCommand
            {
                Telegram = emp.Telegram,
                Message = $"{who} {what} вашу заявку на отпуск с {holiday.Datestart}"
            }));

            await Task.WhenAll(task);

            return await Ok();
        }
    }
}

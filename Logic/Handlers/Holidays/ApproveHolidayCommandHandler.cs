using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;

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
        //todo: send notification
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

            return await Ok();
        }
    }
}

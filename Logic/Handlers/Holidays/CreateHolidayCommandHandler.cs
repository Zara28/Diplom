using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Posts;
using OfficeTime.Logic.Integrations.Refit.Commands;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class CreateHolidayCommandHandler : AbstractCommandHandler<CreateHolidayCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CreateHolidayCommandHandler(
                ILogger<CreateHolidayCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public override async Task<IHandleResult> HandleAsync(CreateHolidayCommand command, CancellationToken cancellationToken = default)
        {
            var today = DateTime.Now;
            var firstdayyear = new DateTime(today.Year, 11, 1);
            var lastdayyear = firstdayyear.AddYears(1).AddDays(-1);
            int count = GetDays(_context.Holidays.Where(h => h.Empid == command.Empid && h.Datestart > firstdayyear && h.Datestart < lastdayyear).ToList());

            if ((bool)command.Pay && count + GetDays((DateTime)command.Datestart, (DateTime)command.Dateend) > 28)
            {
                var task = Task.Run(async () => await _mediator.Send(new NotificationSendCommand
                {
                    Message = $"У вас превышено количество оплачиваемых дней, на данный момент зарегистрировано {count} дней",
                    Telegram = _context.Employees.FirstOrDefault(f => f.Id == command.Empid).Telegram
                }));
                await Task.WhenAll(task);
                return await BadRequest("У вас превышено количество оплачиваемых дней");
            }

            var holiday = new Holiday
            {
                Datestart = command.Datestart,

                Dateend = command.Dateend,

                Pay = command.Pay,

                Isleadapp = false,

                Isdirectorapp = false,

                Datecreate = DateTime.Now,

                Empid = command.Empid
            };

            _context.Holidays.Add(holiday);

            _context.SaveChanges();

            return await Ok();
        }

        private int GetDays(List<Holiday> holidays)
        {
            int count = 0;
            foreach (var holiday in holidays)
            {
                for (var date = holiday.Datestart; date <= holiday.Dateend; date = date.Value.AddDays(1))
                {
                    if (date.Value.DayOfWeek != DayOfWeek.Saturday
                        && date.Value.DayOfWeek != DayOfWeek.Sunday)
                        count++;
                }
            }

            return count;
        }

        private int GetDays(DateTime Start, DateTime End)
        {
            int count = 0;
            for (var date = Start; date <= End; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday)
                    count++;
            }

            return count;
        }
    }
}

using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Posts;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class CreateHolidayCommandHandler : AbstractCommandHandler<CreateHolidayCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public CreateHolidayCommandHandler(
                ILogger<CreateHolidayCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(CreateHolidayCommand command, CancellationToken cancellationToken = default)
        {
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
    }
}

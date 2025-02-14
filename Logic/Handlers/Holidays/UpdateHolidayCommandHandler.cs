using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Posts;
using System.Net;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class UpdateHolidayCommandHandler : AbstractCommandHandler<UpdateHolidayCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public UpdateHolidayCommandHandler(
                ILogger<UpdateHolidayCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(UpdateHolidayCommand command, CancellationToken cancellationToken = default)
        {
            var holiday = new Holiday
            {
                Id = command.Id.Value,
                Datestart = command.Datestart,

                Dateend = command.Dateend,

                Pay = command.Pay,

                Isleadapp = command.Isleadapp,

                Isdirectorapp = command.Isdirectorapp,

                Dateapp = command.Dateapp,

                Empid = command.Empid
            };

            _context.Attach(holiday).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HolidayExists(holiday.Id))
                {
                    return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }

            return await Ok();
        }

        private bool HolidayExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }
    }
}

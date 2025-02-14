using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Posts;
using System.Net;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class DeleteHolidayCommandHandler : AbstractCommandHandler<DeleteHolidayCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public DeleteHolidayCommandHandler(
                ILogger<DeleteHolidayCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(DeleteHolidayCommand command, CancellationToken cancellationToken = default)
        {
            var holiday = await _context.Holidays.FindAsync(command.Id);
            if (holiday != null)
            {
                _context.Holidays.Remove(holiday);
                await _context.SaveChangesAsync();
                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

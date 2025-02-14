using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Holidays;
using System.Net;

namespace OfficeTime.Logic.Handlers.Medicals
{
    public class DeleteMedicalCommandHandler : AbstractCommandHandler<DeleteMedicalCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public DeleteMedicalCommandHandler(
                ILogger<DeleteMedicalCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(DeleteMedicalCommand command, CancellationToken cancellationToken = default)
        {
            var medical = await _context.Medicals.FindAsync(command.Id);
            if (medical != null)
            {
                _context.Medicals.Remove(medical);
                await _context.SaveChangesAsync();
                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

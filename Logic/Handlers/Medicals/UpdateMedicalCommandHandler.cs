using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Holidays;
using System.Net;

namespace OfficeTime.Logic.Handlers.Medicals
{
    public class UpdateMedicalCommandHandler : AbstractCommandHandler<UpdateMedicalCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public UpdateMedicalCommandHandler(
                ILogger<UpdateMedicalCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(UpdateMedicalCommand command, CancellationToken cancellationToken = default)
        {
            var medical = new Medical
            {
                Id = command.Id.Value,
                Datestart = command.Datestart,

                Dateend = command.Dateend
            };

            _context.Attach(medical).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalExists(medical.Id))
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

        private bool MedicalExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }
    }
}

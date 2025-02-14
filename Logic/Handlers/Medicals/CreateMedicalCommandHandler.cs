using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Holidays;

namespace OfficeTime.Logic.Handlers.Medicals
{
    public class CreateMedicalCommandHandler : AbstractCommandHandler<CreateMedicalCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public CreateMedicalCommandHandler(
                ILogger<CreateMedicalCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(CreateMedicalCommand command, CancellationToken cancellationToken = default)
        {
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

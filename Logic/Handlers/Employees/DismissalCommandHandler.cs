using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class DismissalCommandHandler : AbstractCommandHandler<DismissalCommand>
    {
        private readonly diplom_adminkaContext _context;

        public DismissalCommandHandler(diplom_adminkaContext context,
                                        ILogger<DeleteEmployeeCommandHandler> logger,
                                        IMediator mediator) : base(logger, mediator)
        {
            _context = context;
        }

        public override async Task<IHandleResult> HandleAsync(DismissalCommand command, CancellationToken cancellationToken = default)
        {
            await _context.Dismissals.AddAsync(new Dismissal
            {
                Empid = command.Id,
                Date = command.Date,
                Datecreate = DateTime.Now
            });

            _context.SaveChanges();

            return await Ok();
        }
    }
}

using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class DismissalCancelCommandHandler : AbstractCommandHandler<DismissalCancelCommand>
    {
        private readonly diplom_adminkaContext _context;

        public DismissalCancelCommandHandler( ILogger<DismissalCancelCommandHandler> logger,
                                        IMediator mediator,
                                        diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
        }

        public override async Task<IHandleResult> HandleAsync(DismissalCancelCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var delete = _context.Dismissals.FirstOrDefault(d => d.Empid == command.Id);

                _context.Dismissals.Remove(delete);

                _context.SaveChanges();

                return await Ok();
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }
    }
}

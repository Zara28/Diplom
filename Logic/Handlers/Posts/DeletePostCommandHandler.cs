using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Employees;
using System.Net;

namespace OfficeTime.Logic.Handlers.Posts
{
    public class DeletePostCommandHandler : AbstractCommandHandler<DeletePostCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public DeletePostCommandHandler(
                ILogger<DeletePostCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(DeletePostCommand command, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FindAsync(command.Id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return await Ok();
            }

            return await BadRequest("Объект не найден", httpStatusCode: HttpStatusCode.NotFound);
        }
    }
}

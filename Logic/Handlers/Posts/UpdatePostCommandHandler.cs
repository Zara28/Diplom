using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Employees;
using System.Net;

namespace OfficeTime.Logic.Handlers.Posts
{
    public class UpdatePostCommandHandler : AbstractCommandHandler<UpdatePostCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public UpdatePostCommandHandler(
                ILogger<UpdatePostCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(UpdatePostCommand command, CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Id = command.Id,
                Name = command.Name,
                Rate = command.Rate,
            };

            _context.Attach(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
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

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}

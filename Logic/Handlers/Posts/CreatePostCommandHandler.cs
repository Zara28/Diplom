using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Employees;

namespace OfficeTime.Logic.Handlers.Posts
{
    public class CreatePostCommandHandler : AbstractCommandHandler<CreatePostCommand>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public CreatePostCommandHandler(
                ILogger<CreatePostCommandHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Name = command.Name,
                Rate = command.Rate
            };

            _context.Posts.Add(post);

            _context.SaveChanges();

            return await Ok();
        }
    }
}

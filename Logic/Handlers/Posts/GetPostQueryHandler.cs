using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Handlers.Employees;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Handlers.Posts
{
    public class GetPostQueryHandler : AbstractQueryHandler<GetPostQuery, List<PostView>>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public GetPostQueryHandler(
                ILogger<GetEmployeesQueryHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult<List<PostView>>> HandleAsync(GetPostQuery query, CancellationToken cancellationToken)
        {
            if (query.Id.HasValue)
            {
                var emp = _context.Posts.Where(p => p.Id == query.Id).ToList();
                return await Ok(emp.Select(e => _mapper.Map<PostView>(e)).ToList());
            }
            var posts = _context.Posts.ToList();

            if (!String.IsNullOrEmpty(query.Name))
            {
                posts = posts.Where(p => p.Name.ToLower().Contains(query.Name.ToLower())).ToList();
            }

            var result = posts.Select(p => _mapper.Map<PostView>(p)).ToList();

            result.ForEach(p => p.Count = GetCount(p.Id));

            return await Ok(result);
        }

        private int GetCount(int postId)
        {
            return _context.Employees.Where(e => e.Postid == postId).Count();
        }
    }
}

using AutoMapper;
using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Newtonsoft.Json;
using OfficeTime.DBModels;
using OfficeTime.GenerationModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Integrations.Refit.Commands;
using OfficeTime.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OfficeTime.Logic.Handlers.Generation
{
    [TrackedType]
    public class GeneratePostReportCommandHandler : AbstractCommandHandler<GeneratePostReportCommand>
    {
        [Constant(BlockName = "Constants")]
        private static string _fIODirector;
        [Constant(BlockName = "Constants")]
        private static string _companyName;
        [Constant(BlockName = "Constants")]
        private static string _telegramMain;

        private readonly diplom_adminkaContext _context;

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GeneratePostReportCommandHandler(diplom_adminkaContext context,
                                            IMediator mediator,
                                            IMapper mapper,
                                            ILogger<GeneratePostReportCommandHandler> logger) : base(logger, mediator)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<IHandleResult> HandleAsync(GeneratePostReportCommand command, CancellationToken cancellationToken = default)
        {
            var posts = _context.Posts.ToList();

            var result = posts.Select(p => _mapper.Map<PostView>(p)).ToList();

            result.ForEach(p => p.Count = GetCount(p.Id));

            var model = new GenerationModels.Posts
            {
                NameCompany = _companyName,
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now.AddMonths(6),
                PostsRow = result.Select(r => new PostsRow
                {
                    NamePost = r.Name,
                    Count = r.Count,
                    Cost = r.Rate
                }).ToArray(),
                SumCost = result.Sum(s => s.Rate),
                SumCount = result.Sum(s => s.Count),
            };

            var data = JsonConvert.SerializeObject(model);

            await _mediator.Send(new DocumentSendCommand
            {
                InputModel = new Integrations.Refit.Intefaces.InputModel
                {
                    Payload = data,
                    TelegramId = _telegramMain,
                    TypeEnum = TypeEnum.Posts
                }
            });

            return await Ok();
        }

        private int GetCount(int postId)
        {
            return _context.Employees.Where(e => e.Postid == postId).Count();
        }
    }
}

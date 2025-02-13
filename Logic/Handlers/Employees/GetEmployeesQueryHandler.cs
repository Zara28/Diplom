using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Handlers.Employees
{
    public class GetEmployeesQueryHandler : AbstractQueryHandler<GetEmployeesQuery, List<EmployeeView>>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesQueryHandler(
                ILogger<GetEmployeesQueryHandler> logger,
                IMediator mediator, 
                IMapper mapper,
                diplom_adminkaContext context): base(logger, mediator)
        { 
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult<List<EmployeeView>>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            if(query.Id.HasValue)
            {
                var emp = _context.Employees.Where(e => e.Id == query.Id).ToList();
                return await Ok(emp.Select(e => _mapper.Map<EmployeeView>(e)).ToList());
            }
            var employees = _context.Employees.ToList();

            if(!String.IsNullOrEmpty(query.Name))
            {
                employees = employees.Where(e => e.Fio.ToLower().Contains(query.Name.ToLower())).ToList();
            }

            if(query.PostId.HasValue)
            {
                employees = employees.Where(e => e.Post.Id == query.PostId.Value).ToList();
            }

            return await Ok(employees.Select(e => _mapper.Map<EmployeeView>(e)).ToList());
        }
    }
}

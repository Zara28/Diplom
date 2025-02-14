using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                if (query.Id.HasValue)
                {
                    var emp = _context.Employees
                        .Include(e => e.Post)
                        .Where(e => e.Id == query.Id).ToList();
                    return await Ok(emp.Select(e => _mapper.Map<EmployeeView>(e)).ToList());
                }
                var employees = from emp in _context.Employees
                                join dim in _context.Dismissals on emp.Id equals dim.Empid
                                join p in _context.Posts on emp.Postid equals p.Id
                                where !dim.Isapp || !dim.Isapp
                                select emp;

                if (!String.IsNullOrEmpty(query.Name))
                {
                    employees = employees.Where(e => e.Fio.ToLower().Contains(query.Name.ToLower()));
                }

                if (query.PostId.HasValue)
                {
                    employees = employees.Where(e => e.Post.Id == query.PostId.Value);
                }

                return await Ok(employees.Select(e => _mapper.Map<EmployeeView>(e)).ToList());
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }
    }
}

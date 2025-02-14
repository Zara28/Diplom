using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Handlers.Employees;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Handlers.Holidays
{
    public class GetHolidaysQueryHandler : AbstractQueryHandler<GetHolidaysQuery, List<HolidayView>>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public GetHolidaysQueryHandler(
                ILogger<GetHolidaysQueryHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult<List<HolidayView>>> HandleAsync(GetHolidaysQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query.Id.HasValue)
                {
                    var holday = _context.Holidays
                        .Include(h => h.Emp).Where(h => h.Id == query.Id.Value).ToList();
                    return await Ok(holday.Select(h => _mapper.Map<HolidayView>(h)).ToList());
                }
                var holdays = _context.Holidays
                    .Include(h => h.Emp).ToList();

                if (query.EmpId.HasValue)
                {
                    holdays = holdays.Where(h => h.Empid == query.EmpId.Value).ToList();
                }

                if (query.IsPay.HasValue)
                {
                    holdays = holdays.Where(h => h.Pay == query.IsPay.Value).ToList();
                }

                if (query.DateStart.HasValue || query.DateEnd.HasValue)
                {
                    holdays = holdays.Where(h => h.Datestart >= query.DateStart.Value || h.Dateend <= query.DateEnd.Value).ToList();
                }

                if (query.DateStart.HasValue && query.DateEnd.HasValue)
                {
                    holdays = holdays.Where(h => h.Datestart >= query.DateStart.Value && h.Dateend <= query.DateEnd.Value).ToList();
                }

                var result = holdays.Select(p => _mapper.Map<HolidayView>(p)).ToList();

                return await Ok(result);
            }
            catch(Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }
    }
}

using AutoMapper;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeTime.DBModels;
using OfficeTime.Logic.Handlers.Holidays;
using OfficeTime.Logic.Queries;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Handlers.Medicals
{
    public class GetMedicalQueryHandler : AbstractQueryHandler<GetMedicalQuery, List<MedicalView>>
    {
        private readonly diplom_adminkaContext _context;
        private readonly IMapper _mapper;
        public GetMedicalQueryHandler(
                ILogger<GetMedicalQueryHandler> logger,
                IMediator mediator,
                IMapper mapper,
                diplom_adminkaContext context) : base(logger, mediator)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<IHandleResult<List<MedicalView>>> HandleAsync(GetMedicalQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query.Id.HasValue)
                {
                    var medical = _context.Medicals
                        .Include(h => h.Emp).Where(h => h.Id == query.Id.Value).ToList();
                    return await Ok(medical.Select(h => _mapper.Map<MedicalView>(h)).ToList());
                }
                var medicals = _context.Medicals
                    .Include(h => h.Emp).ToList();

                if (query.EmpId.HasValue)
                {
                    medicals = medicals.Where(h => h.Empid == query.EmpId.Value).ToList();
                }

                if (query.DateStart.HasValue || query.DateEnd.HasValue)
                {
                    medicals = medicals.Where(h => h.Datestart >= query.DateStart.Value || h.Dateend <= query.DateEnd.Value).ToList();
                }

                if (query.DateStart.HasValue && query.DateEnd.HasValue)
                {
                    medicals = medicals.Where(h => h.Datestart >= query.DateStart.Value && h.Dateend <= query.DateEnd.Value).ToList();
                }

                var result = medicals.Select(p => _mapper.Map<MedicalView>(p)).ToList();

                return await Ok(result);
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }
    }
}

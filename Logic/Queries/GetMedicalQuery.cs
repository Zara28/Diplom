using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Base;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Queries
{
    public class GetMedicalQuery : BaseFilterQuery, IRequestModel<List<MedicalView>>
    {
        public int? EmpId { get; set; }
    }
}

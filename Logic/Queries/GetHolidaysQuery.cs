using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Base;
using OfficeTime.Logic.Interfaces;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Queries
{
    public class GetHolidaysQuery : BaseFilterQuery, IRequestModel<List<HolidayView>>
    {
        public int? EmpId { get; set; }
        public bool? IsPay { get; set; }
    }
}

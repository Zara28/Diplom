using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class ApproveHolidayCommand : IRequestModel
    {
        public int Id { get; set; }
        public bool IsLead { get; set; }
        public bool Value { get; set; }
    }
}

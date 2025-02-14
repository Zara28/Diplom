using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class CreateHolidayCommand : IRequestModel
    {
        public DateTime? Datestart { get; set; }

        public DateTime? Dateend { get; set; }

        public bool? Pay { get; set; }

        public int Empid { get; set; }
    }
}

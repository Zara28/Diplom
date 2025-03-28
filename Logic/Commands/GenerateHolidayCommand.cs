using Goldev.Core.MediatR.Models;
using Syncfusion.EJ2.Charts;

namespace OfficeTime.Logic.Commands
{
    public class GenerateHolidayCommand : IRequestModel
    {
        public bool Year {  get; set; }
    }
}

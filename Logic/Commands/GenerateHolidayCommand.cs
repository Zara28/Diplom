using Goldev.Core.MediatR.Models;
using OfficeTime.GenerationModels;
using Syncfusion.EJ2.Charts;

namespace OfficeTime.Logic.Commands
{
    public class GenerateHolidayCommand : IRequestModel
    {
        public TypeEnum Type { get; set; }
    }
}

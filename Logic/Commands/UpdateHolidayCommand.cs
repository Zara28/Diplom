using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Interfaces;

namespace OfficeTime.Logic.Commands
{
    public class UpdateHolidayCommand : BaseEntityCommand, IRequestModel
    {
        public DateTime? Datestart { get; set; }

        public DateTime? Dateend { get; set; }

        public bool? Pay { get; set; }

        public bool? Isleadapp { get; set; }

        public bool? Isdirectorapp { get; set; }

        public DateTime? Dateapp { get; set; }

        public int Empid { get; set; }
    }
}

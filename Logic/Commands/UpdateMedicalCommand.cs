using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Interfaces;

namespace OfficeTime.Logic.Commands
{
    public class UpdateMedicalCommand : BaseEntityCommand, IRequestModel
    {
        public DateTime Datestart { get; set; }

        public DateTime Dateend { get; set; }
    }
}

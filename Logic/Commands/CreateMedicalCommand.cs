using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class CreateMedicalCommand : IRequestModel
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int EmpId { get; set; }
    }
}

using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class DeleteEmployeeCommand : IRequestModel
    {
        public int Id { get; set; }
    }
}

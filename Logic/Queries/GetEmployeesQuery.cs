using Goldev.Core.MediatR.Models;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Queries
{
    public class GetEmployeesQuery : IRequestModel<List<EmployeeView>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? PostId { get; set; }
    }
}

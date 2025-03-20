using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class DismissalCommand : IRequestModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class DismissalCancelCommand : IRequestModel
    {
        public int Id { get; set; }
    }
}

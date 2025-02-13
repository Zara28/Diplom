using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Interfaces;

namespace OfficeTime.Logic.Commands
{
    public class UpdatePostCommand : BaseEntityCommand, IRequestModel
    {
        public string Name { get; set; }
        public int Rate { get; set; }
    }
}

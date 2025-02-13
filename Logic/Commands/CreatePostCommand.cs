using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class CreatePostCommand : IRequestModel
    {
        public string Name { get; set; }
        public int Rate { get; set; }
    }
}

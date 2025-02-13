using Goldev.Core.MediatR.Models;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Queries
{
    public class GetPostQuery : IRequestModel<List<PostView>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}

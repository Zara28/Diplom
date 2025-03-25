using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.Logic.Integrations.Refit.Intefaces;

namespace OfficeTime.Logic.Integrations.Refit.Commands
{
    public class DocumentSendCommand : IRequestModel
    {
        public InputModel InputModel { get; set; }
    }
}

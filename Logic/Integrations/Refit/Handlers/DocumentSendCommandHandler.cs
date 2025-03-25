using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.Logic.Handlers.Employees;
using OfficeTime.Logic.Integrations.Refit.Commands;
using OfficeTime.Logic.Integrations.Refit.Intefaces;
using Refit;

namespace OfficeTime.Logic.Integrations.Refit.Handlers
{
    [TrackedType]
    public class DocumentSendCommandHandler : AbstractCommandHandler<DocumentSendCommand>
    {
        [Constant(BlockName = "Constants")]
        private static string _urlDocument;

        public DocumentSendCommandHandler(
                ILogger<DocumentSendCommandHandler> logger,
                IMediator mediator) : base(logger, mediator)
        {
        }
        public override async Task<IHandleResult> HandleAsync(DocumentSendCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var docApi = RestService.For<IGenerateDocument>(_urlDocument);

                await docApi.CreateDocument(command.InputModel);

                return await Ok();
            }
            catch (Exception e)
            {
                return await BadRequest(e.Message);
            }
        }
    }
}

using AutoMapper;
using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using OfficeTime.DBModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Employees;
using OfficeTime.Logic.Integrations.Refit.Commands;
using OfficeTime.Logic.Integrations.Refit.Intefaces;
using Refit;

namespace OfficeTime.Logic.Integrations.Refit.Handlers
{
    [TrackedType]
    public class NotificationSendCommandHandler : AbstractCommandHandler<NotificationSendCommand>
    {
        [Constant(BlockName = "Constants")]
        private static string _urlNotification;

        public NotificationSendCommandHandler(
                ILogger<CreateEmployeeCommandHandler> logger,
                IMediator mediator) : base(logger, mediator)
        {
        }
        public override async Task<IHandleResult> HandleAsync(NotificationSendCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var botApi = RestService.For<INotificationBot>(_urlNotification);

                await botApi.SendMessage(new TelegramMessage
                {
                    ChatId = Convert.ToInt64(command.Telegram),
                    Message = command.Message,
                });

                return await Ok();
            }
            catch(Exception e)
            {
                return await BadRequest(e.Message);
            }
        }
    }
}

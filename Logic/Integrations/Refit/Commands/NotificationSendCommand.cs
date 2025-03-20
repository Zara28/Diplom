using Goldev.Core.MediatR.Models;
using Goldev.Core.Models;

namespace OfficeTime.Logic.Integrations.Refit.Commands
{
    public class NotificationSendCommand : IRequestModel
    {
        public string Telegram {  get; set; }
        public string Message {  get; set; }

    }
}

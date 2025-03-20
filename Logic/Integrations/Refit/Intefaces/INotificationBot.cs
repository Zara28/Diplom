using Microsoft.AspNetCore.Mvc;
using OfficeTime.Logic.Integrations.YandexTracker.Models;
using Refit;

namespace OfficeTime.Logic.Integrations.Refit.Intefaces
{
    public class TelegramMessage
    {
        public long ChatId { get; set; }
        public string Message { get; set; }
    }

    public interface INotificationBot
    {
        [Post("/Send")]
        Task SendMessage([FromBody]TelegramMessage message);
    }
}

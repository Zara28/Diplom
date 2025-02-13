using Goldev.Core.MediatR.Models;

namespace OfficeTime.Logic.Commands
{
    public class CreateEmployeeCommand : IRequestModel
    {
        public int Id { get; set; }

        public string Fio { get; set; }

        public string Telegram { get; set; }

        public string Yandex { get; set; }

        public DateTime? Datebirth { get; set; }

        public DateTime? Datestart { get; set; }

        public int PostId { get; set; }
        public string Password { get; set; }
    }
}

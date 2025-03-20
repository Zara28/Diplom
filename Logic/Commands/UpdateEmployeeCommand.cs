using Goldev.Core.MediatR.Models;
using OfficeTime.Logic.Interfaces;
using OfficeTime.ViewModels;

namespace OfficeTime.Logic.Commands
{
    public class UpdateEmployeeCommand : BaseEntityCommand, IRequestModel
    {
        public string Fio { get; set; }

        public string Telegram { get; set; }

        public string Yandex { get; set; }

        public DateTime? Datebirth { get; set; }

        public DateTime? Datestart { get; set; }

        public int? PostId { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
    }
}

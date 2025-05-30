﻿
using OfficeTime.Mapper;

namespace OfficeTime.ViewModels
{
    public class EmployeeView
    {
        public int Id { get; set; }

        public string Fio { get; set; }

        public string? Telegram { get; set; } = String.Empty;

        public string? Yandex { get; set; } = String.Empty;

        public DateOnly? Datebirth { get; set; }

        public DateOnly? Datestart { get; set; }

        public string? Post { get; set; }
        public string? Password { get; set; } = String.Empty;

        public bool IsDelete { get; set; }
        public DateOnly? DeleteDate { get; set; }
        public RoleAccess Role {  get; set; }
    }
}

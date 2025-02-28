namespace OfficeTime.ViewModels
{
    public class HolidayView
    {
        public int Id { get; set; }

        public DateTime? Datestart { get; set; }

        public DateTime? Dateend { get; set; }

        public bool Pay { get; set; }

        public bool? Isleadapp { get; set; }

        public bool? Isdirectorapp { get; set; }

        public DateTime? Datecreate { get; set; }

        public DateTime? Dateapp { get; set; }

        public string? Emp { get; set; }
    }
}

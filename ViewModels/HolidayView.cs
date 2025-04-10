namespace OfficeTime.ViewModels
{
    public class HolidayView
    {
        public int Id { get; set; }

        public DateOnly? Datestart { get; set; }

        public DateOnly? Dateend { get; set; }

        public bool Pay { get; set; }

        public bool? Isleadapp { get; set; }

        public bool? Isdirectorapp { get; set; }

        public DateOnly? Datecreate { get; set; }

        public DateOnly? Dateapp { get; set; }

        public string? Emp { get; set; }

        public bool? Canceled { get; set; } 
    }
}

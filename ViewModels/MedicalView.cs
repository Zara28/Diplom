namespace OfficeTime.ViewModels
{
    public class MedicalView
    {
        public int Id { get; set; }

        public DateOnly? Datestart { get; set; }

        public DateOnly? Dateend { get; set; }

        public DateOnly? Datecreate { get; set; }

        public string Emp { get; set; }
    }
}

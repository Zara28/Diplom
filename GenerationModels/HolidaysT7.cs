namespace OfficeTime.GenerationModels
{
    public class HolidaysT7
    {
        public string NameCompany { get; set; }
        public string FIODirector {  get; set; }

        public HolidayRow[] Holidays { get; set; }
    }

    public class HolidayRow
    {
        public string Post { get; set; }
        public string FIO { get; set; }
        public string Number { get; set; }
        public int CountDays { get; set; }
        public DateTime DateStart { get; set; }
    }
}

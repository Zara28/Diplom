namespace OfficeTime.GenerationModels
{
    public class PutHolidays
    {
        public string NameComppany { get; set; }
        public string FIODirector { get; set; }
        public List<PutHolidaysRow> Rows { get; set; }
    }
    public class PutHolidaysRow
    {
        public string FIO { get; set; }
        public int Number { get; set; }
        public string Post { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public DateTime DateStartWork { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class PutHoliday
    {
        public string FIO { get; set; }
        public string FIODirector { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}

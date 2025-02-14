using OfficeTime.Logic.Interfaces;

namespace OfficeTime.Logic.Base
{
    public class BaseFilterQuery : BaseEntityCommand
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}

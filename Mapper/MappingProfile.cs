using AutoMapper;
using OfficeTime.DBModels;
using OfficeTime.ViewModels;

namespace OfficeTime.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeView>()
                .ForMember(dest => dest.Post, opt => opt.MapFrom(employee => employee.Post.Name))
                .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(employee => employee.Dismissal != null))
                .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(employee => employee.Dismissal.Date))
                .ReverseMap();

            CreateMap<Holiday, HolidayView>()
                .ForMember(dest => dest.Emp, opt => opt.MapFrom(holiday => holiday.Emp.Fio))
                .ReverseMap();

            CreateMap<Medical, MedicalView>()
                .ForMember(dest => dest.Emp, opt => opt.MapFrom(medical => medical.Emp.Fio))
                .ReverseMap();

            CreateMap<Post, PostView>()
                .ReverseMap();
        }
     }
}

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
                .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Dismissal.Date.GetValueOrDefault())))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(employee => (RoleAccess)Enum.Parse(typeof(RoleAccess), employee.Access.Name)))
                .ForMember(dest => dest.Datestart, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datestart.GetValueOrDefault()))) 
                .ForMember(dest => dest.Datebirth, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datebirth.GetValueOrDefault()))) 
                .ReverseMap();

            CreateMap<Holiday, HolidayView>()
                .ForMember(dest => dest.Emp, opt => opt.MapFrom(holiday => holiday.Emp.Fio))
                .ForMember(dest => dest.Datestart, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datestart.GetValueOrDefault())))
                .ForMember(dest => dest.Dateend, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Dateend.GetValueOrDefault())))
                .ForMember(dest => dest.Dateapp, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Dateapp.GetValueOrDefault())))
                .ForMember(dest => dest.Datecreate, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datecreate.GetValueOrDefault())))
                .ReverseMap();

            CreateMap<Medical, MedicalView>()
                .ForMember(dest => dest.Emp, opt => opt.MapFrom(medical => medical.Emp.Fio))
                .ForMember(dest => dest.Datestart, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datestart.GetValueOrDefault())))
                .ForMember(dest => dest.Dateend, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Dateend.GetValueOrDefault())))
                .ForMember(dest => dest.Datecreate, opt => opt.MapFrom(employee => DateOnly.FromDateTime(employee.Datecreate.GetValueOrDefault())))
                .ReverseMap();

            CreateMap<Post, PostView>()
                .ReverseMap();
        }
     }
}

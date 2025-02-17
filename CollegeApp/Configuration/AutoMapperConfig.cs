using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();

            //Khi có 2 trường khác tên, ví dụ: studentName và Name
            //CreateMap<StudentDTO, Student>().ForMember(n => n.studentName, opt => opt.MapFrom(x => x.Name)).ReverseMap();

            //Khi muốn map tất cả ngoại trừ studentName
            //CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.studentName, opt => opt.Ignore());

            //Khi giá trị bị null
            //CreateMap<StudentDTO, Student>().ReverseMap()
            //.ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No value found" : n.Address));
        }
    }
}

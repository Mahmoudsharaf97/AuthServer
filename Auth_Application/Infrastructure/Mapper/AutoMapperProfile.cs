using Auth_Application.Features;
using Auth_Application.Models;
using Auth_Core;
using AutoMapper;
namespace IdentityApplication.Infrastructure.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterCommand, RegisterInput>().ReverseMap();
          //  CreateMap<BeginEndLoginQuery, LogInInput>().ReverseMap();
            CreateMap<ApplicationUser<string>, RegisterInput>();
        }
    }
}

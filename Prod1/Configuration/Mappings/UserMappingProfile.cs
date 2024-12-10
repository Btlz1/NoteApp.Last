using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
	    CreateMap<CreateUserDto, User>()
		    .ForMember(dest => dest.Id, opt => opt.Ignore())
		    .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login.Trim()))
		    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim()));
	    
	    CreateMap<(int UserId, UpdateUserDto UpdateDto), User>()
		    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
		    .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UpdateDto.Login.Trim()))
		    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UpdateDto.Password.Trim()));
    }
}
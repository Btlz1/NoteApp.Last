using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
	    CreateMap<CreateUserDto, UserVm>()
		    .ForMember(dest
			    => dest.Id, opt
			    => opt.Ignore())
		    .ForMember(dest => dest.Login,
			    opt
				    => opt.MapFrom(dest
					    => dest.Login.Trim()))
		    .ForMember(dest => dest.Password, opt => opt.MapFrom(dest
			    => dest.Password.Trim()));
	        

        CreateMap<(int UserId, UpdateUserDto UpdateDto), UserVm>()
	        .ForMember(dest
		        => dest.Id, opt
		        => opt.MapFrom(dest
			        => dest.UserId))
	        .ForMember(dest => dest.Login,
		        opt
			        => opt.MapFrom(dest
				        => dest.UpdateDto.Login.Trim()))
	        .ForMember(dest => dest.Password, opt => opt.MapFrom(dest
		        => dest.UpdateDto.Password.Trim()));
    }
}
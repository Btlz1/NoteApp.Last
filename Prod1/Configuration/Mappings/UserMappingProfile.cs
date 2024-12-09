using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserVm>();
        
        CreateMap<User, UserListVm>();
       
        CreateMap<IEnumerable<User>, ListOfUsers>()
            .ForCtorParam(nameof(ListOfUsers.Users), 
	           
	            source
		            => source.MapFrom(userList 
			            => userList.ToList()));

        CreateMap<CreateUserDto, User>()
	        .ForMember(dest
		        => dest.Id, opt
		        => opt.Ignore())
	        .ForMember(dest => dest.Login,
		        opt
			        => opt.MapFrom(tuple
				        => tuple.Login.Trim()))
	        .ForMember(dest => dest.Password, opt => opt.MapFrom(tuple
		        => tuple.Password.Trim()));
        
        CreateMap<(int UserId, UpdateUserDto UpdateDto), User>()
            .ForMember(dest 
	            => dest.Id, opt 
	            => opt.MapFrom(tuple 
	            => tuple.UserId))
            .ForMember(dest => dest.Login, 
		            opt 
			            => opt.MapFrom(tuple 
				            => tuple.UpdateDto.Login.Trim()))
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
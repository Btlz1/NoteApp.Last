using AutoMapper;
using btlz.Contracts;

namespace btlz.Configuration.Mappings;

public class NotesMappingProfile : Profile
{
    public NotesMappingProfile()
    {
        CreateMap<CreateNotesDto, NoteVm>()
            .ForMember(dest 
                => dest.Id, opt 
                => opt.Ignore())
            .ForMember(dest => dest.Name, 
                opt 
                    => opt.MapFrom(dest 
                        => dest.Name.Trim()))
            .ForMember(dest => dest.Description, 
                opt 
                    => opt.MapFrom(dest 
                        => dest.Description.Trim()));
        CreateMap<(int NotesId, UpdateNotesDto UpdateDto), NoteVm>()
            .ForMember(dest
                => dest.Id, opt
                => opt.MapFrom(dest
                    => dest.NotesId))
            .ForMember(dest => dest.Name,
                opt
                    => opt.MapFrom(dest
                        => dest.UpdateDto.Name.Trim()))
            .ForMember(dest => dest.Description,
                opt
                    => opt.MapFrom(dest
                        => dest.UpdateDto.Description.Trim()));
    }
}
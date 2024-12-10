using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class NotesMappingProfile : Profile
{
    public NotesMappingProfile()
    {
        CreateMap<CreateNotesDto, Note>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Description, 
                opt => opt.MapFrom(src => src.Description.Trim()));
        
        CreateMap<(int NotesId, UpdateNotesDto UpdateDto), NoteVm>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NotesId))
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.UpdateDto.Name.Trim()))
            .ForMember(dest => dest.Description, 
                opt => opt.MapFrom(src => src.UpdateDto.Description.Trim()));
    }
}
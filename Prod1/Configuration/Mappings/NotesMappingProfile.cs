using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class NotesMappingProfile : Profile
{
    public NotesMappingProfile()
    {
        CreateMap<Note, NotesVm>();
        
        CreateMap<IEnumerable<Note>, ListOfNotes>()
            .ForCtorParam(nameof(ListOfNotes.Notes), 
	           
                source
                    => source.MapFrom(notesList 
                        => notesList.ToList()));
       
        CreateMap<CreateNotesDto, Note>()
            .ForMember(dest 
                => dest.Id, opt 
                => opt.Ignore())
            .ForMember(dest => dest.Name, 
                opt 
                    => opt.MapFrom(tuple 
                        => tuple.Name.Trim()))
            .ForMember(dest => dest.Description, 
                opt 
                    => opt.MapFrom(tuple 
                        => tuple.Description.Trim()));
        CreateMap<(int NotesId, UpdateNotesDto UpdateDto), Note>()
            .ForMember(dest 
                => dest.Id, opt 
                => opt.MapFrom(tuple 
                    => tuple.NotesId))
            .ForMember(dest => dest.Name, 
                opt 
                    => opt.MapFrom(tuple 
                        => tuple.UpdateDto.Name.Trim()))
            .ForMember(dest => dest.Description, 
                opt 
                    => opt.MapFrom(tuple 
                        => tuple.UpdateDto.Description.Trim()));
    }
}
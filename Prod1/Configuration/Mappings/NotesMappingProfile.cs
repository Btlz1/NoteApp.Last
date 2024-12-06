using AutoMapper;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Configuration.Mappings;

public class NotesMappingProfile : Profile
{
    public NotesMappingProfile()
    {
        CreateMap<Notes, NotesVm>();
        CreateMap<Notes, NotesListVm>();
        CreateMap<IEnumerable<Notes>, ListOfNotes>()
            .ForCtorParam(nameof(ListOfNotes.Notes), 
	           
                source
                    => source.MapFrom(notesList 
                        => notesList.ToList()));
       
        CreateMap<CreateNotesDto, Notes>()
            .ForMember(dest 
                => dest.Id, opt 
                => opt.Ignore());
        CreateMap<(int NotesId, UpdateNotesDto UpdateDto), Notes>()
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
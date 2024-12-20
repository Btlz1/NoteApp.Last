namespace btlz.Contracts;

public record NoteVm(int UserId, int Id, string Name, string Description, Enum? Tags);
public record NotesVm(List<NoteVm> Notes);
public record CreateNotesDto(string Name, string Description );
public record UpdateNotesDto(string Name, string Description);
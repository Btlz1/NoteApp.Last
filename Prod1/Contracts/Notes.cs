namespace btlz.Contracts;

public record NoteVm(int Id, string Name, string Description);
public record NotesVm(List<NoteVm> Notes);
public record CreateNotesDto(string Name, string Description );
public record UpdateNotesDto(string Name, string Description);
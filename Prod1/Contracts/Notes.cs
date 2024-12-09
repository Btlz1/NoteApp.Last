namespace btlz.Contracts;

public record NotesVm(int Id, string Name, string Description);
public record ListOfNotes(List<NotesVm> Notes);
public record CreateNotesDto(int UserId ,string Name, string Description );
public record UpdateNotesDto(string Name, string Description);
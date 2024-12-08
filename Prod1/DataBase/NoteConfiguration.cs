using btlz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace btlz.Database;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(note => note.Id);
        builder.Property(user => user.Name).HasMaxLength(128);
        builder.Property(user => user.Description).HasMaxLength(512);
    }
}   
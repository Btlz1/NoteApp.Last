using btlz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace btlz.Database;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
       
        builder.Property(user => user.Login).HasMaxLength(128);
        builder.Property(user => user.Password).HasMaxLength(256);
    }
}
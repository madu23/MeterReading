using ENSEK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ENSEK.Database.Config
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.AccountId);
            builder.Property(x => x.Id)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.AccountId)
                .HasMaxLength(10)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            // setup index
            builder.HasIndex(x => x.AccountId)
                .IsUnique();

            builder.ToTable(nameof(Account));
        }
    }
}

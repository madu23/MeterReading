using ENSEK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Database.Config
{
    public class MeterReadingConfig : IEntityTypeConfiguration<MeterReading>
    {
        public void Configure(EntityTypeBuilder<MeterReading> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.AccountId)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.MeterReadValue)
                .IsRequired();

            builder.Property(x => x.MeterReadingDate)
                .IsRequired();

            // entity relationship mapping
            builder.HasOne(x => x.Account)
                .WithMany(x => x.MeterReadings)
                .HasForeignKey(x => x.AccountId)
                .IsRequired();

            builder.ToTable(nameof(MeterReading));
        }
    }
}

using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    internal class ExcelFileConfiguration : IEntityTypeConfiguration<ExcelFile>
    {
        public void Configure(EntityTypeBuilder<ExcelFile> entity)
        {
            entity.Property(e => e.FileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            entity.Property(e => e.StateDate).HasColumnType("datetime");

            entity.Property(e => e.StateMaker)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.FileStateId)
                    .HasColumnName("FileState");

        }
    }
}

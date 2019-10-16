using Application.Data;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    internal class MfufileConfiguration : IEntityTypeConfiguration<Mfufile>
    {
        public void Configure(EntityTypeBuilder<Mfufile> entity)
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("MFUFile");

            entity.Property(e => e.FileId).HasColumnName("FileID");

            entity.Property(e => e.EnterDate).HasColumnType("datetime");

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}

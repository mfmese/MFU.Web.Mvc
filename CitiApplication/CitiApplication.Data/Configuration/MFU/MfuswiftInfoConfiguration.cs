using Application.Data;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    internal class MfuswiftInfoConfiguration : IEntityTypeConfiguration<MfuswiftInfo>
    {
        public void Configure(EntityTypeBuilder<MfuswiftInfo> entity)
        {
            entity.ToTable("MFUSwiftInfo");

            entity.Property(e => e.Acc).HasMaxLength(100);

            entity.Property(e => e.Act).HasMaxLength(100);

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Diff).HasColumnType("decimal(10, 3)");

            entity.Property(e => e.FileId).HasColumnName("FileID");

            entity.Property(e => e.Isin).HasMaxLength(100);

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Refe).HasMaxLength(100);

            entity.Property(e => e.SettDate).HasColumnType("datetime");

            entity.Property(e => e.TradeDate).HasColumnType("datetime");

            entity.Property(e => e.TradeType).HasMaxLength(100);

        }
    }
}

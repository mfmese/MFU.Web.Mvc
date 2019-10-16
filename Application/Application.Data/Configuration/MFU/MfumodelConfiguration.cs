using Application.Data;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    internal class MfumodelConfiguration : IEntityTypeConfiguration<Mfumodel>
    {
        public void Configure(EntityTypeBuilder<Mfumodel> entity)
        {
            entity.HasKey(e => e.ModelId);

            entity.ToTable("MFUModel");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");

            entity.Property(e => e.AccountNo)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.BeginOfDay).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.ClearingType)
                .HasMaxLength(1)
                .HasDefaultValueSql("(N'')");

            entity.Property(e => e.Columns).HasColumnType("xml");

            entity.Property(e => e.DebtOrMoneyOwedToOne)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.DefineDetail)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ExchangeDate).HasColumnType("datetime");

            entity.Property(e => e.FileId).HasColumnName("FileID");

            entity.Property(e => e.Isin)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Member)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}

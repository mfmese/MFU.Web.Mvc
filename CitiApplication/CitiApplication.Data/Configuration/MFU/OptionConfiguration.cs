using Application.Data;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    internal class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> entity)
        {
            entity.Property(e => e.OptionId).HasColumnName("OptionID");

            entity.Property(e => e.OptionKey)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.OptionValue)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.StateDate).HasColumnType("datetime");

            entity.Property(e => e.StateMaker)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}

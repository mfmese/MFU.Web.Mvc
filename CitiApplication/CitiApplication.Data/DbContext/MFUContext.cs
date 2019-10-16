using Application.Data.Configurations;
using Application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    internal partial class MFUContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MFUContext(DbContextOptions<MFUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExcelFile> ExcelFile { get; set; }
        public virtual DbSet<ExcelFileHistory> ExcelFileHistory { get; set; }
        public virtual DbSet<Mfufile> Mfufile { get; set; }
        public virtual DbSet<Mfumodel> Mfumodel { get; set; }
        public virtual DbSet<MfuswiftInfo> MfuswiftInfo { get; set; }
        public virtual DbSet<Option> Option { get; set; }
        public virtual DbSet<OptionHistory> OptionHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExcelFileConfiguration());
            modelBuilder.ApplyConfiguration(new ExcelFileHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new MfufileConfiguration());
            modelBuilder.ApplyConfiguration(new MfumodelConfiguration());
            modelBuilder.ApplyConfiguration(new MfuswiftInfoConfiguration());
            modelBuilder.ApplyConfiguration(new OptionConfiguration());
            modelBuilder.ApplyConfiguration(new OptionHistoryConfiguration());
        }
    }
}

using System.Data.Entity;
using LicenseGenerator.Models;

namespace LicenseGenerator.DAL
{
    public class LicenseGeneratorContext : DbContext
    {
        public LicenseGeneratorContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            CreateCustomerModel(modelBuilder);
            CreateProductModel(modelBuilder);
            CreateLicenseHistoryModel(modelBuilder);
        }

        private void CreateCustomerModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(p => p.Id);
            modelBuilder.Entity<Customer>().Map(m =>
            {
                m.ToTable("vw_lg_Customer");
                m.Property(p => p.Id).HasColumnName("kh_Id");
                m.Property(p => p.Symbol).HasColumnName("kh_Symbol");
                m.Property(p => p.Name).HasColumnName("adr_Nazwa");
                m.Property(p => p.Nip).HasColumnName("adr_Nip");
            });
        }

        private void CreateProductModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Map(m =>
            {
                m.ToTable("vw_lg_Product");
                m.Property(p => p.Id).HasColumnName("tw_Id");
                m.Property(p => p.Symbol).HasColumnName("tw_Symbol");
                m.Property(p => p.Name).HasColumnName("tw_Nazwa");
                m.Property(p => p.LicenseName).HasColumnName("tw_Pole1");
                m.Property(p => p.NewestVersion).HasColumnName("tw_Pole2");
                m.Property(p => p.IsInStore).HasColumnName("tw_SklepInternet");
            });
        }

        private void CreateLicenseHistoryModel(DbModelBuilder vrpModelBuilder)
        {
            vrpModelBuilder.Entity<GeneratedLicense>().Property(c => c.ProgramName).IsRequired().HasMaxLength(255);
            vrpModelBuilder.Entity<GeneratedLicense>().Property(c => c.Company).IsRequired().HasMaxLength(255);
            vrpModelBuilder.Entity<GeneratedLicense>().Property(c => c.NIP).IsRequired().IsFixedLength().HasMaxLength(10);
            vrpModelBuilder.Entity<GeneratedLicense>().Property(c => c.PartnerNIP).IsFixedLength().HasMaxLength(10);
            vrpModelBuilder.Entity<GeneratedLicense>().Property(c => c.GenerationDate).IsRequired();
        }

        public DbSet<Customer> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<GeneratedLicense> GeneratedLicensesHistory { get; set; }
    }
}
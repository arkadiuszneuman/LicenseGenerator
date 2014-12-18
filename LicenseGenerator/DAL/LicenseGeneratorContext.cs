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

        public DbSet<Customer> Companies { get; set; }
    }
}
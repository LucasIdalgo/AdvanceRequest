using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataBase
{
    public class DesafioContext : DbContext
    {
        public DesafioContext(DbContextOptions<DesafioContext> options) : base(options) { }

        public DbSet<Client> Client { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Installment> Installment { get; set; }
        public DbSet<AdvanceRequest> AdvanceRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(c => c.ClientId);
            modelBuilder.Entity<Contract>().HasKey(c => c.ContractId);
            modelBuilder.Entity<Installment>().HasKey(i => i.InstallmentId);
            modelBuilder.Entity<AdvanceRequest>().HasKey(a => a.AdvanceRequestId);

            modelBuilder.Entity<Contract>().HasMany(c => c.Installments).WithOne(i => i.Contract).HasForeignKey(i => i.ContractId);
        }
    }
}

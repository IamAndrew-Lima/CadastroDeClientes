using Microsoft.EntityFrameworkCore;
using CadastroClientes.Models;

namespace CadastroClientes.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        public DbSet<Clientes> ClientesCadastro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>().ToTable("ClientesCadastro");
        }
    }
}

using FluxoCaixa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Infraestrutura.Contextos
{
    public sealed class FluxoCaixaContexto : DbContext
    {
        public FluxoCaixaContexto(DbContextOptions<FluxoCaixaContexto> contexto) : base(contexto) { }

        public DbSet<Transacao> Transacoes => Set<Transacao>();
        public DbSet<Saldo> Saldos => Set<Saldo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.ToTable("Transacoes");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Valor)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(x => x.Tipo)
                    .IsRequired();

                entity.Property(x => x.Data)
                    .IsRequired();

                entity.HasIndex(x => x.Data);
            });

            modelBuilder.Entity<Saldo>(entity =>
            {
                entity.ToTable("Saldos");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.ValorTotal)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(x => x.TotalCredito)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(x => x.TotalDebito)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(x => x.Data)
                    .IsRequired();

                entity.HasIndex(x => x.Data);
            });
        }
    }
}

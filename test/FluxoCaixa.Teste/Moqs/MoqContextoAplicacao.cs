using FluxoCaixa.Dominio.Entidades;
using FluxoCaixa.Dominio.Enums;
using FluxoCaixa.Infraestrutura.Contextos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace FluxoCaixa.Teste.Moqs
{
    public sealed class MoqContextoAplicacao : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _conexao;

        public MoqContextoAplicacao()
        {
            _conexao = new SqliteConnection("DataSource=:memory:");
            _conexao.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FluxoCaixaContexto>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<FluxoCaixaContexto>(options =>
                {
                    options.UseSqlite(_conexao);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<FluxoCaixaContexto>();
                db.Database.EnsureCreated();

                var transacoes = db.Transacoes.AsNoTracking().ToList();

                if (!transacoes.Any())
                {
                    db.Transacoes.AddRange(new[]
                    {
                        new Transacao(new DateTime(2026, 9, 10), 500.00m, TipoTransacao.Credito),
                        new Transacao(new DateTime(2026, 9, 10), 250.00m, TipoTransacao.Debito)
                    });

                    db.Saldos.AddRange(new[]
                    {
                        new Saldo(new DateTime(2026, 9, 10), 250.00m, 500.00m, 250.00m)
                    });

                    db.SaveChanges();
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _conexao.Dispose();
        }
    }
}

using FluxoCaixa.Dominio.Entidades;
using FluxoCaixa.Dominio.Interfaces.Repositorios;
using FluxoCaixa.Infraestrutura.Contextos;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Infraestrutura.Repositorios
{
    public class SaldoRepositorio : ISaldoRepositorio
    {
        private readonly FluxoCaixaContexto _contexto;

        public SaldoRepositorio(FluxoCaixaContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(Saldo saldo, CancellationToken cancellationToken = default)
        {
            await _contexto.Saldos.AddAsync(saldo, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task<Saldo> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            return await _contexto.Saldos.AsNoTracking().FirstOrDefaultAsync(x => x.Data.Date >= data.Date && x.Data.Date < data.Date.AddDays(1), cancellationToken);
        }
    }
}

using FluxoCaixa.Dominio.Entidades;
using FluxoCaixa.Dominio.Interfaces.Repositorios;
using FluxoCaixa.Infraestrutura.Contextos;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Infraestrutura.Repositorios
{
    public class TransacaoRepositorio : ITransacaoRepositorio
    {
        private readonly FluxoCaixaContexto _contexto;

        public TransacaoRepositorio(FluxoCaixaContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default)
        {
            await _contexto.Transacoes.AddAsync(transacao, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Transacao>> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            return await _contexto.Transacoes.AsNoTracking().Where(x => x.Data.Date >= data.Date && x.Data.Date < data.Date.AddDays(1)).ToListAsync(cancellationToken);
        }
    }
}

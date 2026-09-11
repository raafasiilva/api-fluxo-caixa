using FluxoCaixa.Dominio.Entidades;

namespace FluxoCaixa.Dominio.Interfaces.Repositorios
{
    public interface ITransacaoRepositorio
    {
        Task AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Transacao>> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default);
    }
}

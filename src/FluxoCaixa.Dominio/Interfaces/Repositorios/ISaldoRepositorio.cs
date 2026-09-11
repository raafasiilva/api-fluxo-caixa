using FluxoCaixa.Dominio.Entidades;

namespace FluxoCaixa.Dominio.Interfaces.Repositorios
{
    public interface ISaldoRepositorio
    {
        Task AdicionarAsync(Saldo saldo, CancellationToken cancellationToken = default);
        Task<Saldo> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default);
    }
}

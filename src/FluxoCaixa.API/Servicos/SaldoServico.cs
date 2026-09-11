using FluxoCaixa.API.DTOs;
using FluxoCaixa.Dominio.Entidades;
using FluxoCaixa.Dominio.Interfaces.Repositorios;

namespace FluxoCaixa.API.Servicos
{
    public class SaldoServico
    {
        private readonly ITransacaoRepositorio _transacaoRepositorio;
        private readonly ISaldoRepositorio _saldoRepositorio;

        public SaldoServico(ITransacaoRepositorio transacaoRepositorio, ISaldoRepositorio saldoRepositorio)
        {
            _transacaoRepositorio = transacaoRepositorio;
            _saldoRepositorio = saldoRepositorio;
        }

        public async Task AdicionarSaldoDiarioAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Transacao> transacoes = await _transacaoRepositorio.ObterPorDataAsync(data, cancellationToken);

            decimal totalCredito = transacoes.Where(t => t.Tipo == Dominio.Enums.TipoTransacao.Credito).Sum(t => t.Valor);
            decimal totalDebito = transacoes.Where(t => t.Tipo == Dominio.Enums.TipoTransacao.Debito).Sum(t => t.Valor);

            await _saldoRepositorio.AdicionarAsync(new Saldo(data, totalCredito - totalDebito, totalCredito, totalDebito), cancellationToken);
        }

        public async Task<SaldoDiarioDto> ObterSaldoDiarioPorDataAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            Saldo saldos = await _saldoRepositorio.ObterPorDataAsync(data, cancellationToken);

            if (saldos == null)
                return null;

            return new SaldoDiarioDto
            {
                Data = saldos.Data,
                TotalCredito = saldos.TotalCredito,
                TotalDebito = saldos.TotalDebito,
                ValorTotal = saldos.ValorTotal
            };
        }
    }
}

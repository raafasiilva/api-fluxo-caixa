using FluxoCaixa.API.DTOs;
using FluxoCaixa.Dominio.Entidades;
using FluxoCaixa.Dominio.Interfaces.Repositorios;

namespace FluxoCaixa.API.Servicos
{
    public class TransacaoServico
    {
        private readonly ITransacaoRepositorio _transacaoRepositorio;
        public TransacaoServico(ITransacaoRepositorio transacaoRepositorio)
        {
            _transacaoRepositorio = transacaoRepositorio;
        }

        public async Task AdicionarTransacaoAsync(CriarTransacaoDto transacao, CancellationToken cancellationToken = default)
        {
            await _transacaoRepositorio.AdicionarAsync(new Transacao(transacao.Data, transacao.Valor, transacao.Tipo), cancellationToken);
        }

        public async Task<IReadOnlyCollection<DetalheTransacaoDto>> ObterTransacaoPorDataAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Transacao> transacoes = await _transacaoRepositorio.ObterPorDataAsync(data, cancellationToken);
            return transacoes.Select(t => new DetalheTransacaoDto
            {
                Data = t.Data,
                Valor = t.Valor,
                Tipo = t.Tipo
            }).ToList();
        }
    }
}

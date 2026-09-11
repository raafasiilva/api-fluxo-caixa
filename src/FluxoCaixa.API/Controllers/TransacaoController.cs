using FluxoCaixa.API.DTOs;
using FluxoCaixa.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.API.Controllers
{
    [ApiController]
    [Route("api/transacao")]
    public class TransacaoController : ControllerBase
    {
        /// <summary>
        /// Adiciona uma nova transação.
        /// </summary>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AdicionarTransacaoAsync([FromBody] CriarTransacaoDto transacao, [FromServices] TransacaoServico transacaoServico, CancellationToken cancellationToken)
        {
            await transacaoServico.AdicionarTransacaoAsync(transacao, cancellationToken);
            return Created();
        }

        /// <summary>
        /// Retorna transações de uma data específica.
        /// </summary>
        [HttpGet("{data:datetime}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<DetalheTransacaoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyCollection<DetalheTransacaoDto>>> ObterTransacaoPorDataAsync([FromRoute] DateTime data, [FromServices] TransacaoServico transacaoServico, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<DetalheTransacaoDto> transacoes = await transacaoServico.ObterTransacaoPorDataAsync(data, cancellationToken);
            return Ok(transacoes);
        }
    }
}

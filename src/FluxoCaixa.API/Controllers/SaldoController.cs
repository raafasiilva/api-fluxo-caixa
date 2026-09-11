using FluxoCaixa.API.DTOs;
using FluxoCaixa.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.API.Controllers
{
    [ApiController]
    [Route("api/saldo")]
    public class SaldoController : ControllerBase
    {
        /// <summary>
        /// Retorna saldo consolidado por dia.
        /// </summary>
        [HttpGet("{data:datetime}")]
        [ProducesResponseType(typeof(SaldoDiarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaldoDiarioDto>> ObterSaldoConsolidadoAsync([FromRoute] DateTime data, [FromServices] SaldoServico saldoServico, CancellationToken cancellationToken)
        {
            SaldoDiarioDto saldoDiario = await saldoServico.ObterSaldoDiarioPorDataAsync(data, cancellationToken);
            return Ok(saldoDiario);
        }
    }
}

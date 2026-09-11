using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.API.DTOs
{
    public record class SaldoDiarioDto
    {
        public DateTime Data { get; init; }

        public decimal ValorTotal { get; init; }

        public decimal TotalCredito { get; init; }

        public decimal TotalDebito { get; init; }
    }
}

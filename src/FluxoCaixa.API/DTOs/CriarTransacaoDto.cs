using FluxoCaixa.Dominio.Enums;
using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.API.DTOs
{
    public record class CriarTransacaoDto
    {
        [Required]
        public DateTime Data { get; init; }

        [Required]
        public decimal Valor { get; init; }

        [Required]
        public TipoTransacao Tipo { get; init; }
    }
}

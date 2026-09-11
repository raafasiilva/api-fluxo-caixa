using FluxoCaixa.Dominio.Enums;

namespace FluxoCaixa.Dominio.Entidades
{
    public sealed class Transacao
    {
        public Transacao(DateTime data, decimal valor, TipoTransacao tipo)
        {
            ValidarTipo(tipo);
            ValidarValor(valor);

            Id = Guid.NewGuid();
            Data = data;
            Valor = valor;
            Tipo = tipo;
        }

        public Guid Id { get; private set; }

        public DateTime Data { get; private set; }

        public decimal Valor { get; private set; }

        public TipoTransacao Tipo { get; private set; }

        private static void ValidarValor(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor não pode ser menor ou igual a zero.", nameof(valor));
        }

        private static void ValidarTipo(TipoTransacao tipo)
        {
            if (!Enum.IsDefined(tipo))
                throw new ArgumentException("Tipo de transação inválida.", nameof(tipo));
        }
    }
}

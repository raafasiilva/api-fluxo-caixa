namespace FluxoCaixa.Dominio.Entidades
{
    public sealed class Saldo
    {
        public Saldo(DateTime data, decimal valorTotal, decimal totalCredito, decimal totalDebito)
        {
            Id = Guid.NewGuid();
            Data = data;
            ValorTotal = valorTotal;
            TotalCredito = totalCredito;
            TotalDebito = totalDebito;
        }

        public Guid Id { get; private set; }

        public DateTime Data { get; private set; }

        public decimal ValorTotal { get; init; }

        public decimal TotalCredito { get; init; }

        public decimal TotalDebito { get; init; }
    }
}

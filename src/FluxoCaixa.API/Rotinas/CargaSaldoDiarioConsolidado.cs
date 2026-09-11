using FluxoCaixa.API.Servicos;

namespace FluxoCaixa.API.Rotinas
{
    public sealed class CargaSaldoDiarioConsolidado : BackgroundService
    {
        private readonly IServiceScopeFactory _servicos;
        private readonly ILogger<CargaSaldoDiarioConsolidado> _log;

        public CargaSaldoDiarioConsolidado(IServiceScopeFactory servicos, ILogger<CargaSaldoDiarioConsolidado> log)
        {
            _servicos = servicos;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = ObterProximaExecucao();

                _log.LogInformation("Proxima execução agenda para: {Delay}.", DateTime.Now.Add(delay));

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

                try
                {
                    await ExecutarRotinaAsync(stoppingToken);
                }
                catch (Exception exception)
                {
                    _log.LogError(exception, "Um erro ocorreu durante a consolidação do saldo diário.");
                }
            }
        }

        private async Task ExecutarRotinaAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("Iniciando consolidação do saldo diário.");

            using var escopo = _servicos.CreateScope();
            var service = escopo.ServiceProvider.GetRequiredService<SaldoServico>();

            await service.AdicionarSaldoDiarioAsync(DateTime.Now.Date.AddDays(-1), cancellationToken);

            await Task.CompletedTask;

            _log.LogInformation("Consolidação do saldo diário concluída.");
        }

        private static TimeSpan ObterProximaExecucao()
        {
            var now = DateTime.Now;
            var nextExecution = now.Date.AddDays(1).AddMinutes(5);
            return nextExecution - now;
        }
    }
}

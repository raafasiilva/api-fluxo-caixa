using FluentAssertions;
using FluxoCaixa.API.DTOs;
using FluxoCaixa.Teste.Moqs;
using System.Net;
using System.Net.Http.Json;

namespace FluxoCaixa.Teste.TestesIntegrados
{
    public sealed class SaldoTeste : IClassFixture<MoqContextoAplicacao>
    {
        private readonly HttpClient _http;

        public SaldoTeste(MoqContextoAplicacao moqContexto)
        {
            _http = moqContexto.CreateClient();
        }

        [Fact]
        public async Task Deve_Retornar_Saldo_Diario()
        {
            var date = new DateTime(2026, 9, 10);

            var response = await _http.GetAsync($"/api/saldo/{date:yyyy-MM-dd}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<SaldoDiarioDto>();

            result.Should().NotBeNull();

            result.TotalDebito.Should().Be(250.00m);
            result.TotalCredito.Should().Be(500.00m);
            result.ValorTotal.Should().Be(250.00m);
        }
    }
}

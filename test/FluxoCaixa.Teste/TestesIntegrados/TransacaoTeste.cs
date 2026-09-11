using FluentAssertions;
using FluxoCaixa.API.DTOs;
using FluxoCaixa.Dominio.Enums;
using FluxoCaixa.Teste.Moqs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace FluxoCaixa.Teste.TestesIntegrados
{
    public sealed class TransacaoTeste : IClassFixture<MoqContextoAplicacao>
    {
        private readonly HttpClient _http;

        public TransacaoTeste(MoqContextoAplicacao moqContexto)
        {
            _http = moqContexto.CreateClient();
        }

        [Fact]
        public async Task Deve_Retornar_Sucesso_Criacao_Transacao()
        {
            CriarTransacaoDto novaTransacao = new CriarTransacaoDto
            {
                Data = new DateTime(2026, 9, 11),
                Valor = 500.00m,
                Tipo = TipoTransacao.Credito
            };

            var resultado = await _http.PostAsJsonAsync("/api/transacao", novaTransacao);

            resultado.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Deve_Retornar_Trasacoes()
        {
            var data = new DateTime(2026, 9, 10);
            var resultado = await _http.GetAsync($"/api/transacao/{data:yyyy-MM-dd}");

            resultado.StatusCode.Should().Be(HttpStatusCode.OK);

            var transacoes = await resultado.Content.ReadFromJsonAsync<IReadOnlyCollection<DetalheTransacaoDto>>();

            transacoes.Should().NotBeNull();
        }
    }
}

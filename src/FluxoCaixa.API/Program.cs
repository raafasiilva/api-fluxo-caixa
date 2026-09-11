using FluxoCaixa.API.Rotinas;
using FluxoCaixa.API.Servicos;
using FluxoCaixa.Dominio.Interfaces.Repositorios;
using FluxoCaixa.Infraestrutura.Contextos;
using FluxoCaixa.Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FluxoCaixaContexto>(op => op.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TransacaoServico>();
builder.Services.AddScoped<SaldoServico>();
builder.Services.AddScoped<ITransacaoRepositorio, TransacaoRepositorio>();
builder.Services.AddScoped<ISaldoRepositorio, SaldoRepositorio>();

builder.Services.AddHostedService<CargaSaldoDiarioConsolidado>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FluxoCaixaContexto>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

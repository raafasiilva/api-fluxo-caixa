# FluxoCaixa - Sistema de Gerenciamento de Fluxo de Caixa

## 📋 Descrição

**FluxoCaixa** é uma aplicação .NET 10 que implementa um sistema de gerenciamento de fluxo de caixa. A aplicação registra transações financeiras (créditos e débitos) e consolida os saldos diários de forma automática.

A arquitetura segue o padrão **DDD (Domain-Driven Design)** com separação clara entre apresentação (API), domínio e infraestrutura.

---

## 🏗️ Arquitetura

A solução é composta por **4 projetos principais**:

### 1. **FluxoCaixa.API**
- REST API responsável pela exposição dos endpoints
- **Framework**: ASP.NET Core com SQLite
- **Funcionalidades**:
  - Recebimento de transações
  - Consulta de transações por data
  - Consulta de saldo consolidado diário
  - Serviço background para consolidação automática de saldos

### 2. **FluxoCaixa.Dominio**
- Camada de domínio com as regras de negócio
- **Entidades**:
  - `Transacao`: Representa uma transação financeira (crédito ou débito)
  - `Saldo`: Representa o saldo consolidado de um dia
- **Enum**:
  - `TipoTransacao`: Define os tipos de transação (Crédito, Débito)
- **Interfaces**: Contratos para repositórios

### 3. **FluxoCaixa.Infraestrutura**
- Implementação de persistência de dados
- **Contexto**: `FluxoCaixaContexto` (Entity Framework Core com SQLite)
- **Repositórios**:
  - `TransacaoRepositorio`: Operações com transações
  - `SaldoRepositorio`: Operações com saldos consolidados

### 4. **FluxoCaixa.Teste**
- Testes integrados da aplicação
- **Testes**: Validação de transações e saldos diários

---

## 📊 Modelo de Dados

### Entidade: Transacao
```
- Id (Guid): Identificador único
- Data (DateTime): Data da transação
- Valor (decimal): Valor da transação (sempre > 0)
- Tipo (TipoTransacao): Crédito ou Débito
```

**Validações**:
- Valor deve ser maior que zero
- Tipo deve ser válido (Crédito=1 ou Débito=2)

### Entidade: Saldo
```
- Id (Guid): Identificador único
- Data (DateTime): Data do saldo
- ValorTotal (decimal): Saldo líquido (Crédito - Débito)
- TotalCredito (decimal): Soma de todas as transações de crédito do dia
- TotalDebito (decimal): Soma de todas as transações de débito do dia
```

---

## 🔄 Fluxo de Funcionamento

### 1. **Registro de Transação**
```
POST /api/transacao
{
	"data": "2024-01-15T00:00:00Z",
	"valor": 1000.00,
	"tipo": 1  // 1 = Crédito, 2 = Débito
}
```
- A transação é validada e armazenada no banco de dados
- Retorna status 201 Created

### 2. **Consulta de Transações por Data**
```
GET /api/transacao/{data}
```
- Retorna todas as transações de uma data específica
- Formato de retorno: `DetalheTransacaoDto[]`

### 3. **Consolidação Automática de Saldos**
- Um serviço background (`CargaSaldoDiarioConsolidado`) executa diariamente às **00:05** (5 minutos após meia-noite)
- Processa as transações do dia anterior
- Calcula:
  - **Total de Créditos**: Soma de todas as transações com tipo Crédito
  - **Total de Débitos**: Soma de todas as transações com tipo Débito
  - **Valor Total**: Total de Créditos - Total de Débitos
- Armazena o saldo calculado na tabela de Saldos

### 4. **Consulta de Saldo Diário**
```
GET /api/saldo/{data}
```
- Retorna o saldo consolidado de um dia específico
- Formato de retorno:
```json
{
	"data": "2024-01-15T00:00:00Z",
	"totalCredito": 5000.00,
	"totalDebito": 2000.00,
	"valorTotal": 3000.00
}
```

---

## 🔌 Endpoints da API

### Transações
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/transacao` | Adiciona uma nova transação |
| GET | `/api/transacao/{data}` | Obtém transações de uma data específica |

### Saldo
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/saldo/{data}` | Obtém saldo consolidado de uma data |

---

## 🛠️ Stack Tecnológico

- **.NET**: Versão 10
- **Framework Web**: ASP.NET Core
- **Banco de Dados**: SQLite via Entity Framework Core
- **Documentação API**: OpenAPI/Swagger
- **Testes**: xUnit
- **IDE**: Visual Studio Community 2026

---

## 🚀 Como Executar

### Pré-requisitos
- .NET 10 SDK instalado
- Visual Studio Community 2026 (recomendado)

### Passos
1. Clone o repositório
2. Abra a solução `FluxoCaixa.slnx` no Visual Studio
3. Restaure os pacotes NuGet
4. Execute a aplicação (F5)
5. A API estará disponível em `https://localhost:{porta}`
6. Acesse Swagger em `https://localhost:{porta}/swagger`

### Banco de Dados
- O banco de dados SQLite é criado automaticamente na primeira execução
- Localização: Diretório da aplicação

---

## 🧪 Testes

Execute os testes integrados com:

```powershell
dotnet test test/FluxoCaixa.Teste/FluxoCaixa.Teste.csproj
```

**Testes disponíveis**:
- `SaldoTeste`: Validação de consolidação de saldos
- `TransacaoTeste`: Validação de transações

---

## 📝 Padrões e Princípios

### DDD (Domain-Driven Design)
- Lógica de negócio centralizada na camada de domínio
- Entidades com validações e comportamentos

### Dependency Injection
- Injeção de dependências configurada via `Program.cs`
- Serviços e repositórios registrados no container

### Repository Pattern
- Abstração de acesso a dados através de `ITransacaoRepositorio` e `ISaldoRepositorio`
- Facilita testes e manutenção

### DTOs (Data Transfer Objects)
- `CriarTransacaoDto`: DTO para criação de transações
- `DetalheTransacaoDto`: DTO para resposta de transações
- `SaldoDiarioDto`: DTO para resposta de saldos

### Background Service
- `CargaSaldoDiarioConsolidado`: Serviço background para consolidação automática
- Agendado para executar diariamente às 00:05

---

## 🔍 Responsabilidades por Camada

| Camada | Responsabilidade |
|--------|-----------------|
| **API** | Expor endpoints HTTP, validação de input, orquestração |
| **Serviços** | Lógica de negócio, orquestração de repositórios |
| **Domínio** | Entidades, validações de regras, enums |
| **Infraestrutura** | Persistência, acesso a dados, contexto do EF Core |

---

## 🔐 Validações

- **Valor de Transação**: Deve ser > 0
- **Tipo de Transação**: Deve ser um valor válido do enum
- **Data**: Formato ISO 8601 requerido na API

---

## 📈 Potenciais Melhorias

- [ ] Implementar logging estruturado
- [ ] Adicionar paginação na consulta de transações
- [ ] Implementar caching de saldos
- [ ] Adicionar testes de performance
- [ ] Implementar versionamento de API
- [ ] Adicionar tratamento de erros mais detalhado
- [ ] Separ jobs em uma aplicação dedicada na execução e persistencia.
- [ ] Implementar idempotencia para endpoints de criação de transações.

---

## 📄 Licença

Projeto desenvolvido como exemplo de aplicação bem estruturada em .NET.


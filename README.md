# AspNetCore Defaults SDK 🔧

**Biblioteca de convenções e utilitários para Web APIs ASP.NET Core** — fornece configurações padrão (telemetria, health checks, logging, OpenAPI e outros) para padronizar serviços Web API.

## ✨ O que oferece

- Configuração padrão de OpenTelemetry (tracing + metrics)
- Log HTTP padrão com interceptor para filtrar endpoints sensíveis
- Health checks prontos com endpoints `/health` e `/alive`
- `MapDefaultEndpoints()` que mapeia `/health`, `/alive` e `/app-info`
- Extensões específicas para Web API (`AddWebApiDefaults`, `MapDefaultWebApiEndpoints`)
- Integração com OpenAPI e documentação (`/docs`, `/swagger`) via `Scalar.AspNetCore`

## 🧩 Projetos fornecidos

- `src/AlefCarlos.AspNetCoreDefaults` — extensões genéricas (telemetria, health checks, http logging, service discovery, etc.)
- `src/AlefCarlos.AspNetCoreDefaults.WebApi` — extensões e OpenAPI para Web APIs
- `src/metapackages/AlefCarlos.AspNetCore.WebApi` — metapacote que referencia `AlefCarlos.AspNetCoreDefaults.WebApi`

## Começando — Exemplo rápido 🚀

Exemplo mínimo (ver `samples/webapi-default.cs`):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddWebApiDefaults();

builder.Services.Configure<OpenApiInfo>(opts => opts.Description = "Descrição da API");

var app = builder.Build();

app.UseHttpLogging();
app.UseProblemDetailsWithDefaults();

app.MapGet("/", () => new { Message = "Hello, World!" }).WithName("HelloWorld");

app.MapDefaultWebApiEndpoints();

app.Run();
```

## Endpoints padrão e comportamento 📍

- `GET /health` — readiness (todos os checks)
- `GET /alive` — liveness (apenas checks com tag `"live"`)
- `GET /app-info` — retorna `ApplicationMetadata` (versão, nome, etc.) (`ExcludeFromDescription` no OpenAPI)
- `/docs` — rota de documentação gerada por `Scalar.AspNetCore`

## Logging e filtros 🔇

- Habilita `HttpLogging` com campos úteis (request/response + headers + duration)
- `FilterRequestLoggingInterceptor` desativa logs para endpoints como `/metrics`, `/env`, `/health`, `/alive`, `/docs`, `/swagger`

## OpenTelemetry & exportadores 🛰️

- Por padrão adiciona instrumentação: ASP.NET Core, HttpClient, Runtime
- Configura `OTEL_SERVICE_NAME` se presente (lê da configuração/variáveis de ambiente)
- Para usar OTLP exporter, defina `OTEL_EXPORTER_OTLP_ENDPOINT`; a biblioteca habilita o exporter se a variável estiver presente

## Configuração (exemplos) ⚙️

- Variáveis de ambiente:
  - `OTEL_SERVICE_NAME` — define o nome do serviço para traces
  - `OTEL_EXPORTER_OTLP_ENDPOINT` — habilita OTLP exporter
- `appsettings` (ex.: `samples/webapi-default.settings.json`) controla níveis de log

## Como referenciar

- Referencie o metapacote `src/metapackages/AlefCarlos.AspNetCore.WebApi`
- Em um projeto: adicione `builder.AddWebApiDefaults()` no `Program.cs` e depois `app.MapDefaultWebApiEndpoints()` ao construir o pipeline

## Observações técnicas / decisões 💡

- `AddDefaults()`:
  - adiciona OpenTelemetry, HTTP logging, health checks e service discovery
  - configura `ApplicationMetadata` (informational version / build version)
- `MapDefaultEndpoints()`
  - mapeia `/health`, `/alive`, `/log-level/*` e `/app-info`
- `ProblemDetails` e tratamento de erros são disponibilizados por `AddWebApiDefaults()` e `UseProblemDetailsWithDefaults()`

## Exemplos e execução local

- `samples/webapi-default.cs` — exemplo pronto para rodar com `dotnet run`
- `samples/webapi-default.run.json` — configurações de lançamento (ex.: `OTEL_SERVICE_NAME=sample-api`)

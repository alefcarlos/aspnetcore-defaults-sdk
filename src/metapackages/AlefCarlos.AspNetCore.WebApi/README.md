# AspNetCoreDefaults.WebApi.All 🔧

Biblioteca de convenções e extensões para padronizar Web APIs ASP.NET Core (OpenAPI, ProblemDetails, health checks, logging e OpenTelemetry).

## Objetivo

Fornecer um conjunto mínimo e opinativo de defaults para projetos Web API, facilitando:
- padronização de endpoints (health, alive, app-info)
- integração e configuração de OpenAPI (via `Microsoft.AspNetCore.OpenApi` + `Scalar.AspNetCore`)
- tratamento de erros com `ProblemDetails`
- configuração de logging HTTP e filtros para endpoints sensíveis
- instrumentação OpenTelemetry (tracing e métricas)

## APIs principais
- `WebApplicationBuilder AddWebApiDefaults(this WebApplicationBuilder builder)`
  - chama `AddDefaults()` (telemetria, health checks, logging, service discovery)
  - adiciona `ProblemDetails`
  - registra `OpenApiInfo` a partir de `ApplicationMetadata`
  - registra transformadores de OpenAPI (`OpenApiInfoTransformer`)

- `void UseProblemDetailsWithDefaults(this WebApplication app)`
  - configura `UseExceptionHandler()` e `UseStatusCodePages()`
  - habilita `UseDeveloperExceptionPage()` em `Development`

- `WebApplication MapDefaultWebApiEndpoints(this WebApplication app)`
  - mapeia endpoints padrão: `/health`, `/alive`, `/app-info`
  - mapeia OpenAPI (`MapOpenApi()`) e referência de API (`MapScalarApiReference("/docs")`)

- `WebApplication MapDefaultEndpoints(this WebApplication app)` (do pacote base)
  - configuração de health checks:
    - `/health` → readiness (todos checks)
    - `/alive` → liveness (apenas checks com tag `"live"`)
  - `GET /app-info` → retorna `ApplicationMetadata` (excluído da documentação via `ExcludeFromDescription()`)

## Exemplo rápido (Program.cs)
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configura defaults opinativos para WebApi (telemetria, logging, health checks, OpenAPI)
builder.AddWebApiDefaults();

// Personalize OpenAPI se desejar
builder.Services.Configure<OpenApiInfo>(opts => opts.Description = "Descrição detalhada da API");

var app = builder.Build();

app.UseHttpLogging();
app.UseProblemDetailsWithDefaults();

app.MapGet("/", () => new { Message = "Hello, World!" }).WithName("HelloWorld");

// Mapeia endpoints padrão + OpenAPI + docs
app.MapDefaultWebApiEndpoints();

app.Run();
```

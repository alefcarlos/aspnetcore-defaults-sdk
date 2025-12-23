# AspNetCoreDefaults.WebApi — README técnico 🔧

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

## Integração OpenAPI
- `OpenApiInfo` é exposto via `IOptions<OpenApiInfo>`; o pacote registra um `IConfigureOptions<OpenApiInfo>` que popula `Title` e `Version` automaticamente a partir de `ApplicationMetadata`.

### Configurando `OpenApiInfo` via DI
- Você pode personalizar o `OpenApiInfo` registrando sua própria configuração com `services.Configure<OpenApiInfo>(...)`. As configurações são aplicadas quando o documento OpenAPI é gerado.

```csharp
// Exemplo: adicionar descrição e contato
builder.Services.Configure<OpenApiInfo>(opts =>
{
    opts.Description = "Descrição detalhada da API";
    opts.Contact = new OpenApiContact { Name = "Equipe X", Email = "team@example.com" };
});
```

- O `OpenApiInfoTransformer` atribui `document.Info = options.Value`, portanto quaisquer propriedades definidas via `Configure<OpenApiInfo>` aparecerão na documentação.

- A documentação pública fica disponível em `/docs` (usando `Scalar.AspNetCore`)

## Logging e filtros
- `AddHttpLoggingDefaults()` habilita `HttpLogging` com campos: request/response + headers + duration e `CombineLogs = true`.
- `FilterRequestLoggingInterceptor` reduz logs (define `HttpLoggingFields.None`) para endpoints que expõem dados sensíveis: `/metrics`, `/env`, `/health`, `/alive`, `/docs`, `/swagger`.
- Recomenda-se chamar `app.UseHttpLogging()` no pipeline quando apropriado (ex.: em staging/production conforme níveis de log).

## OpenTelemetry
- Instrumentação adicionada por padrão: ASP.NET Core, HttpClient e Runtime (métricas e traces).
- `OTEL_SERVICE_NAME` (variável de ambiente) sobrescreve o `ApplicationName` usado como fonte de traces.
- Para habilitar OTLP exporter coloque `OTEL_EXPORTER_OTLP_ENDPOINT`; quando presente o exporter OTLP é registrado automaticamente.

## Health checks
- `AddDefaultHealthChecks()` adiciona o check `self` (tagged `live`) que garante liveness por padrão.
- Para readiness/live customizados, adicione checks à coleção `IHealthChecksBuilder` com tags apropriadas.

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

## Como referenciar
- Adicione referência ao projeto `src/AspNetCoreDefaults.WebApi` ou ao metapacote `src/metapackages/AspNetCoreDefaults.WebApi.All`.
- Chame `builder.AddWebApiDefaults()` na construção da aplicação e `app.MapDefaultWebApiEndpoints()` no pipeline.

## Run & Debug (amostra)
- `samples/webapi-default.cs` exemplifica a integração completa. Use `dotnet run` e verifique `/health`, `/alive`, `/app-info` e `/docs`.
- `samples/webapi-default.run.json` contém variáveis de ambiente úteis (`ASPNETCORE_ENVIRONMENT=Development`, `OTEL_SERVICE_NAME=sample-api`).

## Considerações e boas práticas
- Configure níveis de logging via `appsettings.*.json` para controlar dados sensíveis em produção.
- Teste traces e métricas em ambiente de staging antes de habilitar OTLP em produção.

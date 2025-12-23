# AspNetCoreDefaults 🔧

Biblioteca base de convenções e extensões para aplicações ASP.NET Core focadas em Web APIs e serviços: oferece defaults opinativos para telemetria, logging HTTP, health checks, service discovery e configurações de HttpClient.

## Objetivo
Fornecer um conjunto pequeno e reutilizável de padrões para serviços, permitindo que equipes iniciem novas APIs com observabilidade, resiliência e endpoints operacionais configurados de forma consistente.

## Pacote / dependências relevantes
Baseadas no arquivo de projeto (`AspNetCoreDefaults.csproj`):
- `Microsoft.Extensions.AmbientMetadata.Application` (ApplicationMetadata)
- `Microsoft.Extensions.Http.Resilience` (resiliência de HttpClient)
- `Microsoft.Extensions.ServiceDiscovery` (service discovery)
- `OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Instrumentation.*`, `OpenTelemetry.Exporter.OpenTelemetryProtocol` (telemetria)

## APIs principais
- `TBuilder AddDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder`
  - Registra `ApplicationMetadata` (bind em `ambientmetadata:application`, validação em start)
  - Chama `AddHttpLoggingDefaults()`, `ConfigureOpenTelemetry()`, `AddDefaultHealthChecks()` e registra service discovery
  - Configura defaults para `HttpClient` (resiliência e service discovery)

- `TBuilder AddHttpLoggingDefaults<TBuilder>(this TBuilder builder)`
  - Habilita `HttpLogging` com `RequestPropertiesAndHeaders | ResponsePropertiesAndHeaders | Duration`
  - `CombineLogs = true`
  - Registra `FilterRequestLoggingInterceptor` como `IHttpLoggingInterceptor`

- `TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)`
  - Configura logging para incluir mensagem formatada e scopes
  - Adiciona métricas e tracing: AspNetCore, HttpClient e Runtime
  - Exclui rotas de health (`/health` e `/alive`) de tracing
  - Chama mecanismo que registra OTLP exporter caso `OTEL_EXPORTER_OTLP_ENDPOINT` seja definido

- `TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)`
  - Adiciona um health check `self` marcado com tag `live` (garante liveness básico)

- `WebApplication MapDefaultEndpoints(this WebApplication app)`
  - Mapeia `/health` (readiness — todos checks)
  - Mapeia `/alive` (liveness — apenas checks taggeados com `live`)
  - Mapeia `/app-info` que retorna `ApplicationMetadata` (excluído da documentação do OpenAPI via `ExcludeFromDescription()`)

## Comportamentos e detalhes técnicos
- ApplicationMetadata
  - Registrado via `AddOptionsWithValidateOnStart<ApplicationMetadata>()` e `BindConfiguration("ambientmetadata:application")`.
  - `BuildVersion` é inferido de `AssemblyInformationalVersion` → `Assembly.Version` → fallback `0.1.0+unknown-app`.
  - Se `OTEL_SERVICE_NAME` estiver definido, sobrescreve `ApplicationName` usado pela instrumentação.
  - `EnvironmentName` é definido a partir de `IHostEnvironment.EnvironmentName`.

- OpenTelemetry
  - Instrumentação padrão: AspNetCore, HttpClient e Runtime
  - Tracing omite health endpoints para reduzir ruído
  - Se `OTEL_EXPORTER_OTLP_ENDPOINT` estiver presente, registra o exporter OTLP automaticamente

- HttpLogging e interceptor
  - O interceptor `FilterRequestLoggingInterceptor` remove logging (`HttpLoggingFields.None`) para endpoints comuns sensíveis/ruidosos: `/metrics`, `/env`, `/health`, `/alive`, `/docs`, `/swagger`.
  - `CombineLogs = true` ativa logs combinados por request/response

- HttpClient defaults
  - Ao configurar clientes HTTP via helper interno, é aplicada resiliência padrão (`AddStandardResilienceHandler`) e integração com service discovery (`AddServiceDiscovery`).

## Exemplo de uso (Program.cs)
```csharp
var builder = WebApplication.CreateBuilder(args);

// Registra defaults opinativos (telemetria, logging, health checks, service discovery)
builder.AddDefaults();

var app = builder.Build();

// Use o middleware de logging HTTP conforme necessário
app.UseHttpLogging();

// Mapear endpoints operacionais
app.MapDefaultEndpoints();

app.Run();
```

## Variáveis de ambiente e configuração
- `OTEL_SERVICE_NAME` — (opcional) define o nome do serviço para OpenTelemetry
- `OTEL_EXPORTER_OTLP_ENDPOINT` — (opcional) se definido, habilita o OTLP exporter
- Configuração de `ambientmetadata:application` pode fornecer `ApplicationName`, `BuildVersion`, etc.

## Boas práticas e recomendações
- Mantenha `/app-info` protegida em ambientes públicos se retornar dados sensíveis.
- Controle `HttpLogging` por ambiente e níveis de log para evitar vazamento de dados em produção.
- Teste instrumentação e exporters (OTLP) em staging antes de habilitar em produção.
- Para cenários avançados, estenda `AddDefaults()` em sua aplicação para inserir checks, middlewares ou exportadores adicionais.

## Exemplos & testes
- Veja `samples/webapi-default.cs` para uso integrado com `AddWebApiDefaults()` (apenas um nível acima, mas demonstra `AddDefaults()` em ação).

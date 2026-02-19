#:sdk Aspire.AppHost.Sdk@13.1.1
#:property UserSecretsId=bfd49f4e-ccdf-4c22-980e-72d360def663
#:project ../samples/Todo/TodoApi/
#:package Aspire.Hosting.Kubernetes
#:package Aspire.Hosting.PostgreSQL
#:package Aspire.Hosting.Keycloak

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "admin");
var password = builder.AddParameter("password", secret: true);

var k8s = builder.AddKubernetesEnvironment("k8s");

var keycloak = builder.AddKeycloak("keycloak", 8080, username, password)
    .WithRealmImport("./keycloak/realms")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("postgresdb");

var webapi = builder.AddCSharpApp("sample-api", "./webapi-default.cs");

builder.AddProject<Projects.TodoApi>("todo-api")
    .WithHttpProbe(ProbeType.Liveness, "/alive")
    .WithReference(postgresdb)
    .WaitFor(postgres)
    .WaitFor(keycloak)
    .WithEnvironment(context =>
    {
       context.EnvironmentVariables["Authentication__Schemes__Bearer__Authority"] = $"{keycloak.GetEndpoint("https").Url}/realms/local";
       context.EnvironmentVariables["Authentication__Schemes__Bearer__ValidIssuer"] = "";
    });

builder.Build().Run();

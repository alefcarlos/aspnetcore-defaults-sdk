#:sdk Aspire.AppHost.Sdk@13.1.1
#:property UserSecretsId=bfd49f4e-ccdf-4c22-980e-72d360def663
#:project ../samples/Todo/TodoApi/
#:package Aspire.Hosting.Kubernetes
#:package Aspire.Hosting.PostgreSQL

var builder = DistributedApplication.CreateBuilder(args);

var k8s = builder.AddKubernetesEnvironment("k8s");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("postgresdb");

#pragma warning disable ASPIRECSHARPAPPS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var webapi = builder.AddCSharpApp("sample-api", "./webapi-default.cs");
#pragma warning restore ASPIRECSHARPAPPS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

#pragma warning disable ASPIREPROBES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder.AddProject<Projects.TodoApi>("todo-api")
    .WithHttpProbe(ProbeType.Liveness, "/alive")
    .WithReference(postgresdb)
    .WaitFor(postgres);
#pragma warning restore ASPIREPROBES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

builder.Build().Run();

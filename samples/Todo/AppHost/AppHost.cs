var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Projects.TodoApi>("todo-api")
    .WithHttpHealthCheck("/health")
    .WithReference(postgresdb)
    .WaitFor(postgres);


builder.Build().Run();

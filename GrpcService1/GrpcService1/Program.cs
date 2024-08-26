using GrpcService1;
using GrpcService1.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Greeter.GreeterClient>(o => o.Address = new Uri("https://localhost:7259"));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(o => o.AddService("my_service").Build())
    .WithTracing(o => o
        .AddSource("*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter()
        .AddOtlpExporter(o => o.Endpoint = new Uri("http://localhost:4317")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
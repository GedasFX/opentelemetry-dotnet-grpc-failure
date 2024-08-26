using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly Greeter.GreeterClient _client;

    public GreeterService(ILogger<GreeterService> logger, Greeter.GreeterClient client)
    {
        _logger = logger;
        _client = client;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return await _client.ProcessHelloAsync(request, cancellationToken: context.CancellationToken);
    }

    public override Task<HelloReply> ProcessHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
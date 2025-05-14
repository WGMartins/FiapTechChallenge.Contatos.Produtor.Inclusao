using FluentValidation;
using Infrastructure.RabbitMq;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using UseCase.ContatoUseCase.Adicionar;
using UseCase.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

const string serviceName = "TC.Contato.Produto.Inclusao";

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter());

builder.Services.AddScoped<IAdicionarContatoUseCase, AdicionarContatoUseCase>();
builder.Services.AddScoped<IValidator<AdicionarContatoDto>, AdicionarContatoValidator>();

#region RabbitMQ

builder.Services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<ConnectionFactory>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
    return new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
        VirtualHost = settings.VirtualHost,
    };
});

builder.Services.AddSingleton<Func<Task<IConnection>>>(sp =>
{
    var factory = sp.GetRequiredService<ConnectionFactory>();
    return () => factory.CreateConnectionAsync();
});

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.Run();

public partial class Program { };

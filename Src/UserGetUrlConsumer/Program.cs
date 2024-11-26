using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using MongoDB.Driver;
using Shared.Extensions;
using UserGetUrlConsumer;
using UserGetUrlConsumer.Models;
using UserGetUrlConsumer.Models.Settings;

var host = Host.CreateDefaultBuilder(args)
    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "")
    .ConfigureServices((context, services) =>
    {
        var kafkaSettings = context.Configuration.GetSection("Kafka").Get<KafkaSettings>()!;
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            GroupId = Environment.GetEnvironmentVariable("GroupId") ?? "",
            
        };
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = kafkaSettings.SchemaRegistryServer,
        };

        services.AddSingleton(new ConsumerBuilder<Null, UserVisitMessage>(consumerConfig)
            .SetValueDeserializer(new JsonDeserializer<UserVisitMessage>(new CachedSchemaRegistryClient(schemaRegistryConfig)).AsSyncOverAsync())
            .Build());
        services.AddSingleton(service => new MongoClient(context.Configuration.GetConnectionString("TinyUrl")));
        services.AddExportedServices();

        services.AddHostedService<UserGetUrlService>();
    }).Build();

await host.RunAsync(default);
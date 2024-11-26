namespace ReadTinyUrl.Models.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string SchemaRegistryServer { get; set; } = string.Empty;
}

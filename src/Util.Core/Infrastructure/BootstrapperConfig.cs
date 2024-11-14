namespace Util.Infrastructure; 

/// <summary>
/// Clase que se encarga de la configuración inicial del sistema.
/// </summary>
public class BootstrapperConfig {
    /// <summary>
    /// Obtiene o establece un patrón de expresión regular que se utiliza para omitir ciertos ensamblados.
    /// </summary>
    /// <remarks>
    /// Este patrón se utiliza para filtrar los nombres de los ensamblados que no deben ser procesados 
    /// en ciertas operaciones, como el registro o la depuración. La expresión regular incluye 
    /// una lista extensa de nombres de ensamblados comunes que son excluidos por defecto.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el patrón de expresión regular para los ensamblados a omitir.
    /// </value>
    public static string AssemblySkipPattern { get; set; } = "^System|^Mscorlib|^msvcr120|^Netstandard|^Microsoft|^Autofac|^AutoMapper|^EntityFramework|^Newtonsoft|^Castle|^NLog|^Pomelo|^AspectCore|^Xunit|^Nito|^Npgsql|^Exceptionless|^MySqlConnector|^Anonymously Hosted|^libuv|^api-ms|^clrcompression|^clretwrc|^clrjit|^coreclr|^dbgshim|^e_sqlite3|^hostfxr|^hostpolicy|^MessagePack|^mscordaccore|^mscordbi|^mscorrc|sni|sos|SOS.NETCore|^sos_amd64|^SQLitePCLRaw|^StackExchange|^Swashbuckle|WindowsBase|ucrtbase|^DotNetCore.CAP|^MongoDB|^Confluent.Kafka|^librdkafka|^EasyCaching|^RabbitMQ|^Consul|^Dapper|^EnyimMemcachedCore|^Pipelines|^DnsClient|^IdentityModel|^zlib|^NSwag|^Humanizer|^NJsonSchema|^Namotion|^ReSharper|^JetBrains|^NuGet|^ProxyGenerator|^testhost|^MediatR|^Polly|^AspNetCore|^Minio|^SixLabors|^Quartz|^Hangfire|^Handlebars|^Serilog|^WebApiClientCore|^BouncyCastle|^RSAExtensions|^MartinCostello|^Dapr.|^Bogus|^Azure.|^Grpc.|^HealthChecks|^Google|^CommunityToolkit|^Elasticsearch|^ICSharpCode|Enums.NET|^IdentityServer4|JWT|^MathNet|^MK.Hangfire|Mono.TextTemplating|Nest|^NPOI|^Oracle|Spire.Pdf|^FileSignatures";
}
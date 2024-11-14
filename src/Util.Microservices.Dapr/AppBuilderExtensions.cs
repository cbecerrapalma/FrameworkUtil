using Util.Configs;
using Util.Helpers;
using Util.SystemTextJson;

namespace Util.Microservices.Dapr;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega el soporte para Dapr al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agrega Dapr.</param>
    /// <returns>El mismo constructor de la aplicación con Dapr agregado.</returns>
    public static IAppBuilder AddDapr( this IAppBuilder builder ) {
        return builder.AddDapr( null );
    }

    /// <summary>
    /// Agrega el soporte para Dapr al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará Dapr.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de Dapr.</param>
    /// <returns>El constructor de la aplicación con Dapr agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar Dapr utilizando una acción de configuración.
    /// </remarks>
    /// <seealso cref="DaprOptions"/>
    public static IAppBuilder AddDapr( this IAppBuilder builder, Action<DaprOptions> setupAction ) {
        return builder.AddDapr( setupAction, null );
    }

    /// <summary>
    /// Agrega la funcionalidad de Dapr al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de Dapr.</param>
    /// <param name="buildAction">Una acción que permite personalizar la construcción del cliente Dapr.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para incluir Dapr.</returns>
    /// <remarks>
    /// Este método permite la configuración de Dapr en una aplicación, incluyendo la
    /// configuración de servicios y la configuración de la aplicación.
    /// </remarks>
    /// <seealso cref="DaprOptions"/>
    /// <seealso cref="DaprClientBuilder"/>
    public static IAppBuilder AddDapr( this IAppBuilder builder, Action<DaprOptions> setupAction, Action<DaprClientBuilder> buildAction ) {
        builder.CheckNull( nameof( builder ) );
        var options = new DaprOptions();
        setupAction?.Invoke( options );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.AddDaprClient( clientBuilder => {
                clientBuilder.UseJsonSerializationOptions( GetJsonSerializerOptions() );
                buildAction?.Invoke( clientBuilder );
            } );
            if( setupAction != null )
                services.Configure( setupAction );
        } );
        builder.Host.ConfigureAppConfiguration( ( context, configurationBuilder ) => {
            AddDaprSecretStore( configurationBuilder, options, buildAction );
        } );
        return builder;
    }

    /// <summary>
    /// Obtiene las opciones de configuración para la serialización JSON.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> configurado con políticas de nombrado de propiedades,
    /// manejo de números, codificación y conversores personalizados.
    /// </returns>
    /// <remarks>
    /// Las opciones incluyen:
    /// <list type="bullet">
    /// <item>
    /// <description>Propiedades nombradas en formato camelCase.</description>
    /// </item>
    /// <item>
    /// <description>Ignorar propiedades con valores nulos al serializar.</description>
    /// </item>
    /// <item>
    /// <description>Permitir la lectura de números desde cadenas y escribir números como cadenas.</description>
    /// </item>
    /// <item>
    /// <description>Codificación de caracteres Unicode.</description>
    /// </item>
    /// <item>
    /// <description>Conversores personalizados para manejar tipos de fecha y enumeraciones.</description>
    /// </item>
    /// </list>
    /// </remarks>
    private static JsonSerializerOptions GetJsonSerializerOptions() {
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new EnumJsonConverterFactory()
            }
        };
    }

    /// <summary>
    /// Agrega un almacén de secretos de Dapr a la configuración.
    /// </summary>
    /// <param name="configurationBuilder">El constructor de configuración al que se le añadirá el almacén de secretos.</param>
    /// <param name="options">Las opciones de Dapr que contienen el nombre del almacén de secretos.</param>
    /// <param name="buildAction">Una acción que permite configurar el cliente de Dapr.</param>
    /// <remarks>
    /// Este método verifica si el nombre del almacén de secretos está vacío antes de intentar añadirlo.
    /// Si ocurre una excepción durante el proceso y el sistema operativo es Linux, la excepción se vuelve a lanzar.
    /// </remarks>
    private static void AddDaprSecretStore( IConfigurationBuilder configurationBuilder , DaprOptions options, Action<DaprClientBuilder> buildAction ) {
        if ( options.SecretStoreName.IsEmpty() )
            return;
        try {
            configurationBuilder.AddDaprSecretStore( options.SecretStoreName, CreateDaprClient( buildAction ) );
        }
        catch ( Exception ) {
            if ( Common.IsLinux )
                throw;
        }
    }

    /// <summary>
    /// Crea una instancia de <see cref="DaprClient"/> utilizando un <see cref="DaprClientBuilder"/>.
    /// </summary>
    /// <param name="buildAction">Una acción que permite configurar el <see cref="DaprClientBuilder"/> antes de construir el cliente.</param>
    /// <returns>Una instancia configurada de <see cref="DaprClient"/>.</returns>
    /// <remarks>
    /// Este método permite personalizar la configuración del cliente Dapr antes de su creación,
    /// utilizando el patrón de diseño Builder. Se pueden aplicar opciones de serialización JSON
    /// y otras configuraciones a través de la acción proporcionada.
    /// </remarks>
    /// <seealso cref="DaprClient"/>
    /// <seealso cref="DaprClientBuilder"/>
    private static DaprClient CreateDaprClient( Action<DaprClientBuilder> buildAction ) {
        var clientBuilder = new DaprClientBuilder();
        clientBuilder.UseJsonSerializationOptions( GetJsonSerializerOptions() );
        buildAction?.Invoke( clientBuilder );
        return clientBuilder.Build();
    }
}
using Util.Helpers;

namespace Util.Microservices.Dapr.CommandLines;

/// <summary>
/// Representa un comando para detener el Dapr.
/// </summary>
public class DaprStopCommand
{
    private ILogger _logger;
    private string _appId;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprStopCommand"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece un logger nulo para la instancia.
    /// </remarks>
    private DaprStopCommand()
    {
        _logger = NullLogger.Instance;
    }

    /// <summary>
    /// Crea una instancia de <see cref="DaprStopCommand"/> con el identificador de la aplicación especificado.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que se detendrá.</param>
    /// <returns>Una nueva instancia de <see cref="DaprStopCommand"/> configurada con el <paramref name="appId"/> proporcionado.</returns>
    public static DaprStopCommand Create(string appId)
    {
        return new DaprStopCommand().AppId(appId);
    }

    /// <summary>
    /// Establece el identificador de la aplicación.
    /// </summary>
    /// <param name="id">El identificador de la aplicación que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprStopCommand"/>.</returns>
    private DaprStopCommand AppId(string id)
    {
        _appId = id;
        return this;
    }

    /// <summary>
    /// Establece el registrador de logs para el comando Dapr.
    /// </summary>
    /// <param name="logger">El objeto <see cref="ILogger"/> que se utilizará para registrar información.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprStopCommand"/>.</returns>
    public DaprStopCommand Log(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    /// Ejecuta el comando para detener la aplicación.
    /// </summary>
    /// <remarks>
    /// Este método crea un comando de línea que se ejecuta y espera
    /// que la salida coincida con ciertos mensajes para determinar
    /// si la aplicación se detuvo correctamente o si hubo un error.
    /// </remarks>
    public void Execute()
    {
        CreateCommandLine()
            .OutputToMatch("la aplicación se detuvo con éxito")
            .OutputToMatch("no se pudo detener la aplicación con id")
            .Execute();
    }

    /// <summary>
    /// Crea una instancia de <see cref="CommandLine"/> configurada para detener una aplicación Dapr.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="CommandLine"/> con los argumentos necesarios para ejecutar el comando de detención.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el logger configurado para registrar información sobre el comando que se está creando.
    /// </remarks>
    private CommandLine CreateCommandLine()
    {
        return CommandLine.Create("dapr")
            .Log(_logger)
            .Arguments("stop")
            .Arguments("--app-id", _appId);
    }

    /// <summary>
    /// Obtiene el texto de depuración generado por el comando de línea.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene el texto de depuración.
    /// </returns>
    public string GetDebugText()
    {
        return CreateCommandLine().GetDebugText();
    }
}
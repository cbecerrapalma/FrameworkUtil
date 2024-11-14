using System.Net.Sockets;
using Util.Helpers;

namespace Util.Microservices.Dapr.CommandLines;

/// <summary>
/// Representa un comando para ejecutar Dapr.
/// </summary>
public class DaprRunCommand
{
    private ILogger _logger;
    private string _appId;
    private int _appPort;
    private string _appProtocol;
    private int _daprHttpPort;
    private int _daprGrpcPort;
    private int _metricsPort;
    private string _componentsPath;
    private string _configPath;
    private string _project;
    private bool _isUseFreePorts;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprRunCommand"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el logger en una instancia nula por defecto.
    /// </remarks>
    private DaprRunCommand()
    {
        _logger = NullLogger.Instance;
    }

    /// <summary>
    /// Crea una instancia de <see cref="DaprRunCommand"/> con el identificador de la aplicación especificado.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que se utilizará en la instancia de <see cref="DaprRunCommand"/>.</param>
    /// <returns>Una nueva instancia de <see cref="DaprRunCommand"/> configurada con el <paramref name="appId"/> proporcionado.</returns>
    /// <remarks>
    /// Este método es útil para inicializar un comando de ejecución de Dapr con un identificador de aplicación específico.
    /// </remarks>
    public static DaprRunCommand Create(string appId)
    {
        return new DaprRunCommand().AppId(appId);
    }

    /// <summary>
    /// Obtiene el protocolo de la aplicación.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el protocolo de la aplicación.
    /// </returns>
    public string GetAppProtocol()
    {
        return _appProtocol;
    }

    /// <summary>
    /// Obtiene el puerto de la aplicación.
    /// </summary>
    /// <returns>
    /// El puerto configurado para la aplicación.
    /// </returns>
    public int GetAppPort()
    {
        return _appPort;
    }

    /// <summary>
    /// Obtiene el puerto HTTP de Dapr.
    /// </summary>
    /// <returns>
    /// El puerto HTTP de Dapr como un entero.
    /// </returns>
    public int GetDaprHttpPort()
    {
        return _daprHttpPort;
    }

    /// <summary>
    /// Obtiene el puerto gRPC utilizado por Dapr.
    /// </summary>
    /// <returns>
    /// El puerto gRPC configurado para Dapr.
    /// </returns>
    public int GetDaprGrpcPort()
    {
        return _daprGrpcPort;
    }

    /// <summary>
    /// Obtiene el puerto de métricas.
    /// </summary>
    /// <returns>
    /// El puerto de métricas configurado.
    /// </returns>
    public int GetMetricsPort()
    {
        return _metricsPort;
    }

    /// <summary>
    /// Establece el identificador de la aplicación.
    /// </summary>
    /// <param name="id">El identificador de la aplicación que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    private DaprRunCommand AppId(string id)
    {
        _appId = id;
        return this;
    }

    /// <summary>
    /// Establece el registrador de logs para la instancia actual.
    /// </summary>
    /// <param name="logger">El objeto <see cref="ILogger"/> que se utilizará para registrar información.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand Log(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    /// Establece el puerto de la aplicación.
    /// </summary>
    /// <param name="port">El puerto que se asignará a la aplicación.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand AppPort(int port)
    {
        _appPort = port;
        return this;
    }

    /// <summary>
    /// Establece el protocolo de la aplicación.
    /// </summary>
    /// <param name="protocol">El protocolo que se va a establecer para la aplicación.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand AppProtocol(string protocol)
    {
        _appProtocol = protocol;
        return this;
    }

    /// <summary>
    /// Establece el puerto HTTP para Dapr.
    /// </summary>
    /// <param name="port">El puerto HTTP que se desea configurar.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand DaprHttpPort(int port)
    {
        _daprHttpPort = port;
        return this;
    }

    /// <summary>
    /// Establece el puerto gRPC para Dapr.
    /// </summary>
    /// <param name="port">El número de puerto que se utilizará para la comunicación gRPC.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand DaprGrpcPort(int port)
    {
        _daprGrpcPort = port;
        return this;
    }

    /// <summary>
    /// Establece el puerto para las métricas.
    /// </summary>
    /// <param name="port">El número de puerto que se utilizará para las métricas.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand MetricsPort(int port)
    {
        _metricsPort = port;
        return this;
    }

    /// <summary>
    /// Establece la ruta de los componentes para la instancia de Dapr.
    /// </summary>
    /// <param name="path">La ruta que se desea establecer para los componentes.</param>
    /// <returns>La instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand ComponentsPath(string path)
    {
        _componentsPath = path;
        return this;
    }

    /// <summary>
    /// Establece la ruta de configuración para el comando Dapr.
    /// </summary>
    /// <param name="path">La ruta de configuración que se desea establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/> para permitir la encadenación de métodos.</returns>
    public DaprRunCommand ConfigPath(string path)
    {
        _configPath = path;
        return this;
    }

    /// <summary>
    /// Establece el proyecto actual para el comando Dapr.
    /// </summary>
    /// <param name="project">El nombre del proyecto que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand Project(string project)
    {
        _project = project;
        return this;
    }

    /// <summary>
    /// Establece si se deben utilizar puertos libres para la ejecución de Dapr.
    /// </summary>
    /// <param name="isUseFreePorts">Indica si se deben utilizar puertos libres. El valor predeterminado es verdadero.</param>
    /// <returns>Devuelve la instancia actual de <see cref="DaprRunCommand"/>.</returns>
    public DaprRunCommand UseFreePorts(bool isUseFreePorts = true)
    {
        _isUseFreePorts = isUseFreePorts;
        return this;
    }

    /// <summary>
    /// Ejecuta el comando para inicializar Dapr y verifica su estado.
    /// </summary>
    /// <remarks>
    /// Este método crea una línea de comando que se ejecuta y espera una salida específica que indica
    /// que Dapr se ha inicializado correctamente y está en estado de ejecución.
    /// </remarks>
    public void Execute()
    {
        CreateCommandLine()
            .OutputToMatch("dapr inicializado. Estado: En ejecución.")
            .Execute();
    }

    /// <summary>
    /// Crea una línea de comandos para ejecutar Dapr con los parámetros configurados.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="CommandLine"/> que representa la línea de comandos generada.
    /// </returns>
    /// <remarks>
    /// Este método verifica si se deben utilizar puertos libres y, en caso afirmativo, los obtiene. 
    /// Luego, construye la línea de comandos con los argumentos necesarios para ejecutar Dapr,
    /// incluyendo el identificador de la aplicación, los puertos y otras configuraciones opcionales.
    /// </remarks>
    private CommandLine CreateCommandLine()
    {
        if (_isUseFreePorts)
            GetFreePorts();
        return CommandLine.Create("dapr")
            .Log(_logger)
            .Arguments("run")
            .Arguments("--app-id", _appId)
            .ArgumentsIf(_appPort != 0, "--app-port", _appPort.ToString(CultureInfo.InvariantCulture))
            .ArgumentsIf(_appProtocol.IsEmpty() == false, "--app-protocol", _appProtocol)
            .ArgumentsIf(_daprHttpPort != 0, "--dapr-http-port", _daprHttpPort.ToString(CultureInfo.InvariantCulture))
            .ArgumentsIf(_daprGrpcPort != 0, "--dapr-grpc-port", _daprGrpcPort.ToString(CultureInfo.InvariantCulture))
            .ArgumentsIf(_metricsPort != 0, "--metrics-port", _metricsPort.ToString(CultureInfo.InvariantCulture))
            .ArgumentsIf(_componentsPath.IsEmpty() == false, "--resources-path", _componentsPath)
            .ArgumentsIf(_configPath.IsEmpty() == false, "--config", _configPath)
            .Arguments("--log-level", "debug")
            .ArgumentsIf(_project.IsEmpty() == false, "--")
            .ArgumentsIf(_project.IsEmpty() == false, "dotnet", "run")
            .ArgumentsIf(_project.IsEmpty() == false, "--project", _project)
            .ArgumentsIf(_project.IsEmpty() == false && _appPort != 0, "--urls", $"http://localhost:{_appPort.ToString(CultureInfo.InvariantCulture)}");
    }

    /// <summary>
    /// Obtiene puertos libres en la máquina local para ser utilizados por los listeners.
    /// </summary>
    /// <remarks>
    /// Este método crea un número definido de listeners TCP en la dirección de loopback,
    /// los inicia para obtener puertos disponibles y luego los detiene. Los puertos obtenidos
    /// se asignan a variables de instancia específicas si aún no han sido configuradas.
    /// </remarks>
    /// <exception cref="SocketException">
    /// Se lanzará una excepción si hay un error al intentar iniciar un listener TCP.
    /// </exception>
    private void GetFreePorts()
    {
        const int NUM_LISTENERS = 4;
        IPAddress ip = IPAddress.Loopback;
        var listeners = new TcpListener[NUM_LISTENERS];
        var ports = new int[NUM_LISTENERS];
        for (int i = 0; i < NUM_LISTENERS; i++)
        {
            listeners[i] = new TcpListener(ip, 0);
            listeners[i].Start();
            ports[i] = ((IPEndPoint)listeners[i].LocalEndpoint).Port;
        }
        for (int i = 0; i < NUM_LISTENERS; i++)
        {
            listeners[i].Stop();
        }
        if (_appPort == 0)
            _appPort = ports[0];
        if (_daprHttpPort == 0)
            _daprHttpPort = ports[1];
        if (_daprGrpcPort == 0)
            _daprGrpcPort = ports[2];
        if (_metricsPort == 0)
            _metricsPort = ports[3];
    }

    /// <summary>
    /// Obtiene el texto de depuración generado por el comando.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el texto de depuración.
    /// </returns>
    public string GetDebugText()
    {
        return CreateCommandLine().GetDebugText();
    }
}
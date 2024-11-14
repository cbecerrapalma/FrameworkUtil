namespace Util.Helpers;

/// <summary>
/// Representa una clase que maneja la entrada de comandos desde la línea de comandos.
/// </summary>
public class CommandLine
{
    private ILogger _logger;
    private string _command;
    private string _arguments;
    private readonly IDictionary<string, string> _environmentVariables;
    private bool _redirectStandardOutput;
    private bool _redirectStandardError;
    private Encoding _outputEncoding;
    private bool _useShellExecute;
    private string _workingDirectory;
    private bool _createNoWindow;
    private TimeSpan _timeout;
    private readonly EventWaitHandle _outputReceived;
    private readonly List<string> _outputToMatch;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CommandLine"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor configura los valores predeterminados para las propiedades de la clase,
    /// incluyendo la configuración del logger, redirección de salida estándar y error estándar,
    /// codificación de salida, y variables de entorno.
    /// </remarks>
    public CommandLine()
    {
        _logger = NullLogger.Instance;
        _redirectStandardOutput = true;
        _redirectStandardError = true;
        _outputEncoding = Encoding.UTF8;
        _useShellExecute = false;
        _createNoWindow = true;
        _environmentVariables = new Dictionary<string, string>();
        _timeout = TimeSpan.FromSeconds(30);
        _outputReceived = new EventWaitHandle(false, EventResetMode.ManualReset);
        _outputToMatch = new List<string>();
    }

    /// <summary>
    /// Crea una instancia de <see cref="CommandLine"/> con el comando y los argumentos especificados.
    /// </summary>
    /// <param name="command">El comando que se desea ejecutar.</param>
    /// <param name="arguments">Los argumentos opcionales que se pasarán al comando. Si no se proporcionan, se establecerán como null.</param>
    /// <returns>Una nueva instancia de <see cref="CommandLine"/> configurada con el comando y los argumentos proporcionados.</returns>
    public static CommandLine Create(string command, string arguments = null)
    {
        return new CommandLine().Command(command).Arguments(arguments);
    }

    /// <summary>
    /// Establece el registrador de logs para la instancia actual.
    /// </summary>
    /// <param name="logger">El registrador de logs a utilizar. Si es nulo, se utilizará un registrador nulo por defecto.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/>.</returns>
    public CommandLine Log(ILogger logger)
    {
        _logger = logger ?? NullLogger.Instance;
        return this;
    }

    /// <summary>
    /// Establece el comando que se va a ejecutar.
    /// </summary>
    /// <param name="command">El comando que se desea establecer.</param>
    /// <returns>La instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    public CommandLine Command(string command)
    {
        _command = command;
        return this;
    }

    /// <summary>
    /// Evalúa una condición y, si es verdadera, agrega los argumentos proporcionados.
    /// </summary>
    /// <param name="condition">La condición que se evalúa. Si es falsa, no se agregan argumentos.</param>
    /// <param name="arguments">Una lista de argumentos que se agregarán si la condición es verdadera.</param>
    /// <returns>Retorna la instancia actual de <see cref="CommandLine"/>.</returns>
    public CommandLine ArgumentsIf(bool condition, params string[] arguments)
    {
        if (condition == false)
            return this;
        return Arguments(arguments);
    }

    /// <summary>
    /// Establece los argumentos de línea de comandos.
    /// </summary>
    /// <param name="arguments">Una matriz de cadenas que representan los argumentos a establecer.</param>
    /// <returns>El objeto <see cref="CommandLine"/> actual con los argumentos establecidos.</returns>
    /// <remarks>
    /// Este método permite pasar un número variable de argumentos como una matriz. 
    /// Si se pasa un valor nulo, se devuelve el objeto actual sin cambios.
    /// </remarks>
    public CommandLine Arguments(params string[] arguments)
    {
        if (arguments == null)
            return this;
        return Arguments((IEnumerable<string>)arguments);
    }

    /// <summary>
    /// Establece los argumentos de la línea de comandos si se cumple una condición.
    /// </summary>
    /// <param name="condition">Indica si se deben aplicar los argumentos.</param>
    /// <param name="arguments">Colección de argumentos que se aplicarán si la condición es verdadera.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/>.</returns>
    public CommandLine ArgumentsIf(bool condition, IEnumerable<string> arguments)
    {
        if (condition == false)
            return this;
        return Arguments(arguments);
    }

    /// <summary>
    /// Agrega argumentos de línea de comandos a la colección actual.
    /// </summary>
    /// <param name="arguments">Una colección de cadenas que representan los argumentos a agregar.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/>.</returns>
    /// <remarks>
    /// Si la colección de argumentos es nula, no se realiza ninguna acción. 
    /// Si ya existen argumentos en la colección, se agrega un espacio antes de añadir los nuevos argumentos.
    /// </remarks>
    public CommandLine Arguments(IEnumerable<string> arguments)
    {
        if (arguments == null)
            return this;
        if (_arguments.IsEmpty() == false)
            _arguments += " ";
        _arguments += string.Join(" ", arguments);
        return this;
    }

    /// <summary>
    /// Establece una variable de entorno con una clave y un valor especificados.
    /// </summary>
    /// <param name="key">La clave de la variable de entorno que se desea establecer.</param>
    /// <param name="value">El valor de la variable de entorno que se desea establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. Si la clave ya existe, se elimina antes de agregar la nueva entrada.
    /// </remarks>
    public CommandLine Env(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (_environmentVariables.ContainsKey(key))
            _environmentVariables.Remove(key);
        _environmentVariables.Add(key, value);
        return this;
    }

    /// <summary>
    /// Establece las variables de entorno utilizando un diccionario de pares clave-valor.
    /// </summary>
    /// <param name="env">Un diccionario que contiene las variables de entorno a establecer, donde la clave es el nombre de la variable y el valor es su valor correspondiente.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="env"/> es nulo, el método simplemente devuelve la instancia actual sin realizar ninguna acción.
    /// </remarks>
    public CommandLine Env(IDictionary<string, string> env)
    {
        if (env == null)
            return this;
        foreach (var item in env)
            Env(item.Key, item.Value);
        return this;
    }

    /// <summary>
    /// Configura si se debe redirigir la salida estándar del proceso.
    /// </summary>
    /// <param name="value">Indica si se debe redirigir la salida estándar. El valor predeterminado es true.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si <paramref name="value"/> es true, se deshabilita el uso de la ejecución de shell, 
    /// permitiendo la redirección de la salida estándar. Si es false, se habilita el uso de 
    /// la ejecución de shell y se establece la codificación de salida en null.
    /// </remarks>
    public CommandLine RedirectStandardOutput(bool value = true)
    {
        _redirectStandardOutput = value;
        if (value)
        {
            _useShellExecute = false;
            return this;
        }
        _useShellExecute = true;
        _outputEncoding = null;
        return this;
    }

    /// <summary>
    /// Establece si se debe redirigir el flujo de error estándar.
    /// </summary>
    /// <param name="value">Indica si se debe redirigir el flujo de error estándar. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    public CommandLine RedirectStandardError(bool value = true)
    {
        _redirectStandardError = value;
        return this;
    }

    /// <summary>
    /// Establece la codificación de salida para la línea de comandos.
    /// </summary>
    /// <param name="encoding">La codificación que se utilizará para la salida.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/>.</returns>
    public CommandLine OutputEncoding(Encoding encoding)
    {
        _outputEncoding = encoding;
        return this;
    }

    /// <summary>
    /// Establece si se debe utilizar el shell del sistema operativo para ejecutar el proceso.
    /// </summary>
    /// <param name="value">Un valor booleano que indica si se debe utilizar el shell. El valor predeterminado es verdadero.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si <paramref name="value"/> es verdadero, la salida estándar no se redirige y la codificación de salida se establece en nula.
    /// Si <paramref name="value"/> es falso, la salida estándar se redirige.
    /// </remarks>
    public CommandLine UseShellExecute(bool value = true)
    {
        _useShellExecute = value;
        if (value)
        {
            _redirectStandardOutput = false;
            _outputEncoding = null;
            return this;
        }
        _redirectStandardOutput = true;
        return this;
    }

    /// <summary>
    /// Establece el directorio de trabajo para el comando.
    /// </summary>
    /// <param name="value">El directorio de trabajo que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    public CommandLine WorkingDirectory(string value)
    {
        _workingDirectory = value;
        return this;
    }

    /// <summary>
    /// Establece si se debe crear una ventana para el proceso de línea de comandos.
    /// </summary>
    /// <param name="value">Un valor booleano que indica si se debe crear una ventana. 
    /// Si es <c>true</c>, no se creará una ventana; si es <c>false</c>, se creará una ventana.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/>.</returns>
    public CommandLine CreateNoWindow(bool value)
    {
        _createNoWindow = value;
        return this;
    }

    /// <summary>
    /// Establece un tiempo de espera para el comando.
    /// </summary>
    /// <param name="timeout">El tiempo de espera a establecer como un objeto <see cref="TimeSpan"/>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    public CommandLine Timeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    /// <summary>
    /// Agrega una serie de cadenas de salida a la lista de salidas a coincidir.
    /// </summary>
    /// <param name="outputs">Una colección de cadenas que representan las salidas a agregar.</param>
    /// <returns>Devuelve la instancia actual de <see cref="CommandLine"/> para permitir la encadenación de métodos.</returns>
    public CommandLine OutputToMatch(params string[] outputs)
    {
        _outputToMatch.AddRange(outputs);
        return this;
    }

    /// <summary>
    /// Ejecuta un proceso basado en el comando configurado.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="Process"/> que representa el proceso ejecutado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el comando no ha sido configurado.
    /// </exception>
    /// <exception cref="Exception">
    /// Se lanza cuando el proceso excede el tiempo de espera especificado.
    /// </exception>
    /// <remarks>
    /// Este método inicia un proceso y comienza a leer su salida estándar y de error.
    /// Se utiliza un mecanismo de espera para asegurarse de que el proceso se complete dentro de un tiempo determinado.
    /// </remarks>
    public Process Execute()
    {
        if (_command.IsEmpty())
            throw new ArgumentException("El comando no está configurado.");
        _logger.LogDebug($"Running command: {GetDebugText()}");
        var process = new Process
        {
            StartInfo = CreateProcessStartInfo()
        };
        process.OutputDataReceived += OnOutput;
        process.ErrorDataReceived += OnOutput;
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        var done = _outputReceived.WaitOne(_timeout);
        if (!done)
            throw new Exception($"El comando \"{GetDebugText()}\" ha excedido el tiempo de espera.");
        return process;
    }

    /// <summary>
    /// Crea una instancia de <see cref="ProcessStartInfo"/> configurada con los parámetros necesarios.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ProcessStartInfo"/> que contiene la configuración para iniciar un proceso.
    /// </returns>
    /// <remarks>
    /// Este método escapa los argumentos para asegurar que se manejen correctamente las comillas. 
    /// También configura la redirección de salida estándar y de error, así como otras propiedades 
    /// relacionadas con la ejecución del proceso.
    /// </remarks>
    private ProcessStartInfo CreateProcessStartInfo()
    {
        var escapedArgs = _arguments.Replace("\"", "\\\"");
        var result = new ProcessStartInfo(_command, escapedArgs)
        {
            RedirectStandardOutput = _redirectStandardOutput,
            RedirectStandardError = _redirectStandardError,
            StandardOutputEncoding = _outputEncoding,
            StandardErrorEncoding = _outputEncoding,
            UseShellExecute = _useShellExecute,
            WorkingDirectory = _workingDirectory,
            CreateNoWindow = _createNoWindow,
        };
        foreach (var item in _environmentVariables)
            result.EnvironmentVariables.Add(item.Key, item.Value);
        return result;
    }

    /// <summary>
    /// Maneja la salida de un proceso en ejecución.
    /// </summary>
    /// <param name="sendingProcess">El proceso que está enviando la salida.</param>
    /// <param name="e">Los datos recibidos del proceso.</param>
    /// <remarks>
    /// Este método se invoca cuando hay datos disponibles en la salida estándar del proceso.
    /// Si los datos no están vacíos, se registran en el logger y se verifica si coinciden
    /// con alguna de las cadenas esperadas.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si ocurre un error al intentar registrar los datos.
    /// </exception>
    private void OnOutput(object sendingProcess, DataReceivedEventArgs e)
    {
        if (e.Data.IsEmpty())
            return;
        try
        {
            _logger.LogDebug(e.Data);
        }
        catch (InvalidOperationException)
        {
        }
        if (_outputToMatch.Any(e.Data.Contains))
            _outputReceived.Set();
    }

    /// <summary>
    /// Obtiene una representación en texto de depuración del comando y sus argumentos.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene el comando y los argumentos asociados, separados por un espacio.
    /// </returns>
    public string GetDebugText()
    {
        return $"{_command} {_arguments}";
    }

    /// <summary>
    /// Ejecuta un comando de PowerShell en un proceso de línea de comandos.
    /// </summary>
    /// <param name="command">El comando de PowerShell que se desea ejecutar.</param>
    /// <param name="workingDirectory">El directorio de trabajo en el que se ejecutará el comando. Si es <c>null</c>, se utilizará el directorio de trabajo predeterminado.</param>
    /// <remarks>
    /// Este método crea un nuevo proceso que ejecuta <c>cmd.exe</c> y redirige la entrada y salida estándar para permitir la ejecución de comandos de PowerShell.
    /// El comando se envía a través de la entrada estándar y el proceso espera a que se complete la ejecución antes de continuar.
    /// </remarks>
    /// <exception cref="System.Exception">Lanza una excepción si ocurre un error al iniciar el proceso o al ejecutar el comando.</exception>
    public static void ExecutePowerShell(string command, string workingDirectory = null)
    {
        using Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.Start();
        process.StandardInput.WriteLine($"powershell {command}");
        process.StandardInput.WriteLine("exit");
        process.StandardInput.AutoFlush = true;
        process.WaitForExit();
    }
}
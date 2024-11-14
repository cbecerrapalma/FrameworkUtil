using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Configuration;
using Util.Helpers;
using Util.Ui.Razor.Internal;

namespace Util.Ui.Razor;

/// <summary>
/// Proporciona un servicio para observar cambios en archivos Razor.
/// </summary>
/// <remarks>
/// Este servicio implementa la interfaz <see cref="IRazorWatchService"/> y permite
/// la detección de cambios en los archivos Razor para facilitar el desarrollo en tiempo real.
/// </remarks>
public class RazorWatchService : IRazorWatchService
{
    /// <summary>
    /// Obtiene o establece un valor que indica si el inicio está completo.
    /// </summary>
    /// <value>
    /// <c>true</c> si el inicio está completo; de lo contrario, <c>false</c>.
    /// </value>
    public static bool IsStartComplete { get; set; }
    private readonly FileWatcher _watcher;
    private readonly RazorViewContainer _container;
    private readonly HttpClient _client;
    private readonly IActionDescriptorCollectionProvider _provider;
    private readonly RazorOptions _options;
    private bool _isGenerateMissingHtml;
    private readonly SequentialSimpleAsyncSubject<string> _addViewSubject;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorWatchService"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="resolver">El resolvedor de rutas de vista que se utilizará para encontrar las vistas.</param>
    /// <param name="client">El cliente HTTP que se utilizará para realizar solicitudes.</param>
    /// <param name="provider">El proveedor de colección de descriptores de acción que se utilizará para acceder a los descriptores de acción.</param>
    /// <param name="options">Las opciones de configuración para el servicio Razor.</param>
    /// <remarks>
    /// Este constructor configura el servicio Razor para que observe cambios en las vistas y maneje la adición de nuevas vistas.
    /// </remarks>
    public RazorWatchService(IServiceProvider serviceProvider, IPartViewPathResolver resolver, HttpClient client,
            IActionDescriptorCollectionProvider provider, IOptions<RazorOptions> options)
    {
        Ioc.SetServiceProviderAction(() => serviceProvider);
        _watcher = new FileWatcher();
        _client = client;
        _provider = provider;
        _options = options.Value;
        _container = new RazorViewContainer(resolver);
        _addViewSubject = new SequentialSimpleAsyncSubject<string>();
        client.Timeout = options.Value.StartupTimeout;
        InitAddViewSubject();
    }

    /// <summary>
    /// Inicializa la vista para agregar un nuevo sujeto.
    /// </summary>
    /// <remarks>
    /// Este método se suscribe a los cambios en la lista de sujetos y 
    /// realiza acciones cuando se detecta un nuevo archivo. 
    /// Se registra el evento de descubrimiento de un nuevo archivo, 
    /// se agrega la vista correspondiente al contenedor, se 
    /// habilitan y deshabilitan ciertas funcionalidades de la interfaz 
    /// y se realiza una solicitud asíncrona.
    /// </remarks>
    private void InitAddViewSubject()
    {
        _addViewSubject.Where(path => path.IsEmpty() == false)
            .Delay(TimeSpan.FromSeconds(1))
            .SubscribeAsync(async path =>
            {
                WriteLog($"Nuevos archivos encontrados: {path}");
                _container.AddView(path);
                EnableOverrideHtml(false);
                await Request(path);
                EnableOverrideHtml();
                await Task.Delay(200);
            });
    }

    /// <summary>
    /// Habilita o deshabilita la sobreescritura de HTML en las opciones.
    /// </summary>
    /// <param name="isOverrideHtml">Indica si se debe habilitar la sobreescritura de HTML. El valor predeterminado es true.</param>
    protected void EnableOverrideHtml(bool isOverrideHtml = true)
    {
        _options.EnableOverrideHtml = isOverrideHtml;
    }

    /// <summary>
    /// Realiza una solicitud HTTP asíncrona a una ruta específica.
    /// </summary>
    /// <param name="path">La ruta del recurso que se desea solicitar.</param>
    /// <param name="isWrite">Indica si se debe registrar la solicitud. Por defecto es verdadero.</param>
    /// <param name="times">El número de intentos realizados hasta el momento. Por defecto es 0.</param>
    /// <remarks>
    /// Este método intentará realizar la solicitud hasta un máximo de 3 veces en caso de fallos.
    /// Si la respuesta es un código de estado 404 (No encontrado), se intentará agregar la vista correspondiente.
    /// En caso de una excepción de operación inválida, se esperará 2 segundos antes de reintentar la solicitud.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    /// <seealso cref="AddView(string)"/>
    public async Task Request(string path, bool isWrite = true, int times = 0)
    {
        try
        {
            times++;
            if (times > 3)
                return;
            await Task.Delay(TimeSpan.FromMilliseconds(_options.HtmlRenderDelayOnRazorChange));
            var requestPath = Url.JoinPath(GetApplicationUrl(), "view", path.RemoveStart(_options.RazorRootDirectory).RemoveEnd(".cshtml"));
            WriteLog($"Enviar solicitud: {requestPath}", isWrite);
            var response = await _client.GetAsync(requestPath);
            if (response.IsSuccessStatusCode)
            {
                WriteLog($"Solicitud exitosa: {requestPath}", isWrite);
                return;
            }
            await Task.Delay(2000);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                await AddView(path);
                return;
            }
            await Request(path, isWrite, times);
        }
        catch (InvalidOperationException)
        {
            await Task.Delay(2000);
            await Request(path, isWrite, times);
        }
    }

    /// <summary>
    /// Obtiene la URL de la aplicación a partir de la configuración especificada en el archivo 'launchSettings.json'.
    /// </summary>
    /// <returns>
    /// La URL de la aplicación como una cadena, o null si no se encuentra la clave 'applicationUrl'.
    /// </returns>
    /// <remarks>
    /// Este método busca en la sección de propiedades del proyecto para localizar el archivo de configuración 
    /// 'launchSettings.json' y extraer la URL de la aplicación. Si la clave 'applicationUrl' no está presente, 
    /// se devolverá null.
    /// </remarks>
    protected string GetApplicationUrl()
    {
        var path = GetProjectPath("Properties");
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("launchSettings.json", true, false);
        var config = builder.Build();
        foreach (var child in config.AsEnumerable())
        {
            if (child.Key.Contains("applicationUrl", StringComparison.OrdinalIgnoreCase))
                return child.Value;
        }
        return null;
    }
    /// <summary>
    /// Agrega una vista utilizando la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta de la vista que se va a agregar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de agregar la vista.</returns>
    protected async Task AddView(string path)
    {
        await _addViewSubject.OnNextAsync(path);
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia el proceso de inicialización de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa el resultado asíncrono de la operación de inicio.</returns>
    /// <remarks>
    /// Este método realiza varias operaciones de inicialización, incluyendo la generación de HTML y la preparación de recursos.
    /// Si se produce una excepción durante el proceso, se registra el mensaje de error y se vuelve a lanzar la excepción.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    /// <seealso cref="GenerateAllHtml"/>
    /// <seealso cref="GenerateMissingHtml"/>
    /// <seealso cref="Preheat"/>
    /// <seealso cref="StartWatch"/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        WriteLog("Iniciando....");
        WriteLog("Preparándose para inicializar....");
        InitRazorViewContainer();
        await Task.Factory.StartNew(async () =>
        {
            await Task.Delay(_options.StartInitDelay, cancellationToken);
            try
            {
                await GenerateAllHtml();
                await GenerateMissingHtml();
                await Preheat();
                IsStartComplete = true;
                WriteLog("Inicialización completada..");
                await StartWatch();
            }
            catch (Exception exception)
            {
                WriteLog(exception.Message);
                throw;
            }

        }, cancellationToken);
    }

    /// <summary>
    /// Escribe un registro en la consola con un contenido específico si se permite la escritura.
    /// </summary>
    /// <param name="content">El contenido que se va a registrar en la consola.</param>
    /// <param name="isWrite">Un valor que indica si se debe escribir el registro. Por defecto es verdadero.</param>
    /// <remarks>
    /// Si <paramref name="isWrite"/> es falso, el método no realizará ninguna acción.
    /// El registro incluirá la fecha y hora actual en milisegundos, seguido de un mensaje específico y el contenido proporcionado.
    /// </remarks>
    private void WriteLog(string content, bool isWrite = true)
    {
        if (isWrite == false)
            return;
        Console.WriteLine($"dbug: {DateTime.Now.ToMillisecondString()} - Marco de aplicación Util - Servicio de escucha de Razor - {content}");
    }

    /// <summary>
    /// Inicializa el contenedor de vistas Razor.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de configurar el contenedor de vistas utilizando todas las rutas de vistas disponibles.
    /// </remarks>
    protected virtual void InitRazorViewContainer()
    {
        _container.Init(GetAllViewPaths());
    }

    /// <summary>
    /// Obtiene una lista de todas las rutas de vista disponibles.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que representan las rutas relativas de las vistas, 
    /// sin duplicados.
    /// </returns>
    /// <remarks>
    /// Este método utiliza los descriptores de acción de página para 
    /// recopilar las rutas relativas de las vistas y devuelve una lista 
    /// única de estas rutas.
    /// </remarks>
    protected List<string> GetAllViewPaths()
    {
        var result = new List<string>();
        var descriptors = GetPageActionDescriptors();
        foreach (var descriptor in descriptors)
            result.Add(descriptor.RelativePath);
        return result.Distinct().ToList();
    }

    /// <summary>
    /// Obtiene una lista de descriptores de acción de página.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="PageActionDescriptor"/> que representan los descriptores de acción de página.
    /// </returns>
    protected List<PageActionDescriptor> GetPageActionDescriptors()
    {
        return _provider.ActionDescriptors.Items.OfType<PageActionDescriptor>().ToList();
    }

    /// <summary>
    /// Genera todos los archivos HTML si la opción está habilitada.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la generación de HTML está habilitada en las opciones. 
    /// Si está habilitada, se envía una solicitud a la API para regenerar todos los archivos HTML.
    /// Se registran mensajes en el log para indicar el progreso y el resultado de la operación.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de generación de HTML.
    /// </returns>
    protected async Task GenerateAllHtml()
    {
        if (_options.EnableGenerateAllHtml == false)
            return;
        WriteLog("准备重新生成全部html...");
        _isGenerateMissingHtml = true;
        EnableGenerateHtml();
        var requestPath = Url.JoinPath(GetApplicationUrl(), "api/html");
        var response = await _client.GetAsync(requestPath);
        if (response.IsSuccessStatusCode)
        {
            WriteLog("Regenerar todo el HTML completado....");
            return;
        }
        var result = await response.Content.ReadAsStringAsync();
        WriteLog("Regenerar todo el HTML falló....");
        WriteLog(result);
    }

    /// <summary>
    /// Genera los archivos HTML que faltan en el proyecto, si la opción correspondiente está deshabilitada.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la generación de todos los archivos HTML está habilitada. Si no lo está,
    /// procede a buscar los archivos HTML faltantes en las rutas de vista principales del contenedor.
    /// Para cada ruta que no tenga un archivo HTML correspondiente, se realiza una solicitud para generarlo.
    /// Al finalizar, se restablecen las configuraciones de generación de HTML.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de generación de HTML faltante.
    /// </returns>
    protected async Task GenerateMissingHtml()
    {
        if (_options.EnableGenerateAllHtml)
            return;
        WriteLog("Preparar para generar el HTML faltante....");
        EnableGenerateHtml();
        EnableOverrideHtml(false);
        var fileBasePath = GetProjectPath(_options.RazorRootDirectory);
        var files = Util.Helpers.File.GetAllFiles(fileBasePath, "*.html");
        foreach (var path in _container.GetMainViewPaths())
        {
            if (Exists(files.Select(t => t.FullName).ToList(), path))
                continue;
            _isGenerateMissingHtml = true;
            await Request(path);
        }
        EnableOverrideHtml();
        WriteLog("生成缺失的html完成...");
    }

    /// <summary>
    /// Habilita o deshabilita la generación de HTML.
    /// </summary>
    /// <param name="isGenerateHtml">
    /// Un valor booleano que indica si se debe generar HTML. 
    /// El valor predeterminado es verdadero.
    /// </param>
    protected void EnableGenerateHtml(bool isGenerateHtml = true)
    {
        _options.IsGenerateHtml = isGenerateHtml;
    }

    /// <summary>
    /// Obtiene la ruta completa del proyecto a partir de una ruta relativa.
    /// </summary>
    /// <param name="relativePath">La ruta relativa que se desea convertir en una ruta completa.</param>
    /// <returns>La ruta completa del proyecto como una cadena de texto.</returns>
    /// <remarks>
    /// Este método utiliza el directorio actual del sistema y lo combina con la ruta relativa proporcionada.
    /// </remarks>
    protected string GetProjectPath(string relativePath)
    {
        return Url.JoinPath(Common.GetCurrentDirectory(), relativePath);
    }

    /// <summary>
    /// Verifica si una ruta Razor existe en una lista de rutas HTML.
    /// </summary>
    /// <param name="htmlPaths">Una lista de rutas HTML donde se buscará la existencia de la ruta Razor.</param>
    /// <param name="razorPath">La ruta Razor que se desea verificar.</param>
    /// <returns>
    /// Devuelve true si la ruta Razor es igual a la ruta de error predeterminada o si existe en la lista de rutas HTML; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método compara la ruta Razor proporcionada con una ruta de error específica y luego verifica si la ruta generada a partir de la ruta Razor existe en la lista de rutas HTML.
    /// </remarks>
    protected bool Exists(List<string> htmlPaths, string razorPath)
    {
        if (string.Equals(razorPath, $"{_options.RazorRootDirectory}/Error.cshtml", StringComparison.OrdinalIgnoreCase))
            return true;
        var path = GenerateHtmlFilter.GetPath(razorPath.RemoveStart(_options.RazorRootDirectory).RemoveEnd(".cshtml"), _options);
        return htmlPaths.Any(t => t.Replace("\\", "/").EndsWith(path, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Precalienta las páginas Razor si está habilitado en las opciones.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la generación de HTML faltante está habilitada y si el precalentamiento está activado.
    /// Si ambas condiciones son verdaderas, se procede a realizar el precalentamiento de las páginas.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de precalentamiento.
    /// </returns>
    protected async Task Preheat()
    {
        if (_isGenerateMissingHtml)
            return;
        if (_options.EnablePreheat == false)
            return;
        WriteLog("Preparación de la página Razor para precalentamiento....");
        EnableGenerateHtml(false);
        var paths = _container.GetRandomPaths();
        foreach (var path in paths)
            await Request(path, false);
        WriteLog("La precarga de la página Razor se ha completado....");
    }

    /// <summary>
    /// Inicia el proceso de monitoreo de cambios en archivos Razor (.cshtml).
    /// </summary>
    /// <remarks>
    /// Este método configura un observador de archivos que detecta cambios y renombramientos en archivos 
    /// con la extensión .cshtml en el directorio especificado. Cuando se detecta un cambio, se llama al 
    /// método <see cref="GenerateAsync(string)"/> para procesar el archivo modificado.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de inicio del monitoreo.
    /// </returns>
    protected virtual Task StartWatch()
    {
        WriteLog("Comenzar a escuchar....");
        var path = GetProjectPath(_options.RazorRootDirectory.RemoveStart("/"));
        _watcher.Path(path)
            .Filter("*.cshtml")
            .OnChangedAsync(async (_, e) => await GenerateAsync(e.FullPath))
            .OnRenamedAsync(async (_, e) => await GenerateAsync(e.FullPath))
            .Start();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Genera de manera asíncrona un archivo HTML a partir de la ruta completa proporcionada.
    /// </summary>
    /// <param name="fullPath">La ruta completa del archivo que se va a procesar.</param>
    /// <remarks>
    /// Este método verifica si la ruta debe ser ignorada. Si no es así, habilita la generación de HTML,
    /// registra el archivo modificado y obtiene las rutas de vista asociadas. Si no se encuentran rutas de vista,
    /// se añade una nueva vista. De lo contrario, se realiza una solicitud para cada ruta de vista encontrada.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de generación de HTML.
    /// </returns>
    /// <seealso cref="IsIgnore(string)"/>
    /// <seealso cref="EnableGenerateHtml()"/>
    /// <seealso cref="_container.GetViewPaths(string)"/>
    /// <seealso cref="AddView(string)"/>
    /// <seealso cref="Request(string)"/>
    private async Task GenerateAsync(string fullPath)
    {
        if (IsIgnore(fullPath))
            return;
        EnableGenerateHtml();
        var file = new FileInfo(fullPath);
        var path = file.FullName.Replace(Common.GetCurrentDirectory(), "").Replace("\\", "/");
        WriteLog($"发现修改: {file.FullName.Replace("\\", "/")}");
        var viewPaths = _container.GetViewPaths(path);
        if (viewPaths == null || viewPaths.Count == 0)
        {
            await AddView(path);
            return;
        }
        foreach (var viewPath in viewPaths)
            await Request(viewPath);
    }

    /// <summary>
    /// Determina si una ruta de archivo debe ser ignorada en función de su extensión y contenido.
    /// </summary>
    /// <param name="path">La ruta del archivo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la ruta debe ser ignorada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Esta función ignora archivos que no tienen la extensión ".cshtml" o que contienen 
    /// "_ViewImports" en su ruta. Se utiliza para filtrar archivos en operaciones específicas 
    /// donde solo se desean incluir archivos de vista de Razor.
    /// </remarks>
    protected bool IsIgnore(string path)
    {
        if (path.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase) == false)
            return true;
        if (path.Contains("_ViewImports", StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Detiene de manera asíncrona el servicio.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de detención del servicio.
    /// </returns>
    /// <remarks>
    /// Este método realiza las operaciones necesarias para cerrar el servicio de manera segura,
    /// incluyendo la liberación de recursos como el observador (_watcher).
    /// </remarks>
    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        WriteLog("准备关闭服务...");
        _watcher.Dispose();
        return Task.CompletedTask;
    }
}
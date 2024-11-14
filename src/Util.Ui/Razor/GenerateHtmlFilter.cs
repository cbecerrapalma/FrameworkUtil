using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Util.Helpers;
using File = System.IO.File;

namespace Util.Ui.Razor;

/// <summary>
/// Clase que implementa un filtro de página asíncrono para generar contenido HTML.
/// </summary>
/// <remarks>
/// Esta clase permite modificar la respuesta de una página antes de que se envíe al cliente,
/// proporcionando la capacidad de inyectar HTML adicional o realizar otras transformaciones.
/// </remarks>
public class GenerateHtmlFilter : IAsyncPageFilter
{
    /// <summary>
    /// Método que se ejecuta durante la ejecución de un controlador de página.
    /// </summary>
    /// <param name="context">El contexto de ejecución del controlador de página que contiene información sobre la solicitud actual.</param>
    /// <param name="next">El delegado que representa el siguiente controlador de página en la cadena de ejecución.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método permite realizar acciones antes y después de la ejecución del controlador de página.
    /// Se puede utilizar para implementar lógica de autorización, registro o manipulación de datos.
    /// </remarks>
    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        await next();
    }

    /// <summary>
    /// Maneja la selección de un controlador de página y genera HTML si está habilitado en las opciones.
    /// </summary>
    /// <param name="context">El contexto de selección del controlador de página que contiene información sobre la solicitud actual.</param>
    /// <remarks>
    /// Este método se ejecuta de manera asíncrona y verifica si la generación de HTML está habilitada.
    /// Si el controlador de acción actual es la página de error o si el camino generado es nulo o vacío, el método termina sin realizar ninguna acción.
    /// Si la opción de anulación de HTML está deshabilitada, se verifica si el archivo HTML ya existe antes de proceder a generarlo.
    /// En caso de éxito, se registra un mensaje de depuración; si ocurre un error, se registra un mensaje de error.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        var options = context.HttpContext.RequestServices.GetService<IOptions<RazorOptions>>()?.Value ?? new RazorOptions();
        if (options.IsGenerateHtml == false)
            return;
        if (context.ActionDescriptor.ViewEnginePath == "/Error")
            return;
        var path = CreatePath(context, options);
        if (string.IsNullOrWhiteSpace(path))
            return;
        if (options.EnableOverrideHtml == false)
        {
            var filePath = Util.Helpers.Web.GetPhysicalPath(path);
            if (File.Exists(filePath))
                return;
        }
        var log = GetLogger(context);
        try
        {
            var html = await GetHtml(context);
            await WriteFile(path, html);
            log.LogDebug($"Razor genera HTML con éxito: Razor Path: {context.ActionDescriptor.ViewEnginePath}, Html Path: {path}");
        }
        catch (Exception exception)
        {
            log.LogError(exception, $"La generación de HTML en la página Razor falló: razor path: {context.ActionDescriptor.ViewEnginePath}");
            throw;
        }
    }

    /// <summary>
    /// Obtiene un logger para la clase <see cref="GenerateHtmlFilter"/>.
    /// </summary>
    /// <param name="context">El contexto del manejador de página que contiene información sobre la solicitud actual.</param>
    /// <returns>
    /// Un objeto <see cref="ILogger{T}"/> que se utiliza para registrar información, advertencias y errores.
    /// Si no se puede obtener el logger, se devuelve una instancia de <see cref="NullLogger{T}"/>.
    /// </returns>
    protected virtual ILogger<GenerateHtmlFilter> GetLogger(PageHandlerSelectedContext context)
    {
        return context?.HttpContext?.RequestServices?.GetService<ILogger<GenerateHtmlFilter>>() ?? NullLogger<GenerateHtmlFilter>.Instance;
    }

    /// <summary>
    /// Crea una ruta basada en el contexto de la acción y las opciones de Razor proporcionadas.
    /// </summary>
    /// <param name="context">El contexto del manejador de la página que contiene información sobre la acción actual.</param>
    /// <param name="options">Las opciones de Razor que pueden influir en la creación de la ruta.</param>
    /// <returns>
    /// Una cadena que representa la ruta creada. 
    /// Devuelve una cadena vacía si el atributo HTML está configurado para ser ignorado 
    /// o si la ruta está en blanco. 
    /// Si no se encuentra un atributo HTML, se utiliza la ruta del motor de vistas.
    /// </returns>
    private string CreatePath(PageHandlerSelectedContext context, RazorOptions options)
    {
        var attribute = context?.ActionDescriptor?.ModelTypeInfo?.GetCustomAttribute<HtmlAttribute>();
        if (attribute == null)
            return GetPath(context?.ActionDescriptor.ViewEnginePath, options);
        if (attribute.Ignore)
            return string.Empty;
        return string.IsNullOrWhiteSpace(attribute.Path) ? string.Empty : attribute.Path;
    }

    /// <summary>
    /// Obtiene la ruta completa para un archivo HTML basado en la ruta proporcionada y las opciones de Razor.
    /// </summary>
    /// <param name="path">La ruta del archivo sin la extensión .html. Puede ser relativa o absoluta.</param>
    /// <param name="options">Las opciones de configuración de Razor que contienen información sobre el directorio raíz y la carpeta de generación de HTML.</param>
    /// <returns>
    /// La ruta completa al archivo HTML generado, o una cadena vacía si la ruta proporcionada es nula o solo contiene espacios en blanco.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la ruta contiene un separador de directorios. Si no lo contiene, se asume que es un archivo en la raíz del directorio especificado.
    /// Si la ruta contiene un separador, se extrae el nombre del archivo y se construye la ruta completa en función de la estructura de directorios.
    /// </remarks>
    /// <seealso cref="RazorOptions"/>
    public static string GetPath(string path, RazorOptions options)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;
        if (path.Contains("/") == false)
            return Util.Helpers.Url.JoinPath(options.RazorRootDirectory, options.GenerateHtmlFolder, $"{path}.html");
        var lastIndex = path.LastIndexOf("/", StringComparison.Ordinal);
        return Util.Helpers.Url.JoinPath(options.RazorRootDirectory, path.Substring(0, lastIndex), options.GenerateHtmlFolder, $"{path.Substring(lastIndex + 1)}.html");
    }

    /// <summary>
    /// Obtiene el contenido HTML de una página Razor utilizando el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto de la selección de la página que contiene información sobre la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es el contenido HTML generado.</returns>
    /// <remarks>
    /// Este método utiliza el motor de vistas Razor para renderizar una página y devolver su contenido HTML como una cadena.
    /// Se asegura de que el modelo y el contexto de la vista estén correctamente configurados antes de ejecutar la página.
    /// </remarks>
    /// <seealso cref="PageHandlerSelectedContext"/>
    private async Task<string> GetHtml(PageHandlerSelectedContext context)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var engine = serviceProvider.GetService<IRazorViewEngine>();
        var activator = serviceProvider.GetService<IRazorPageActivator>();
        dynamic model = System.Convert.ChangeType(context.HandlerInstance, context.HandlerInstance.GetType(), CultureInfo.InvariantCulture);
        var actionContext = new ActionContext(context.HttpContext, model.RouteData, context.ActionDescriptor);
        var page = FindPage(engine, context.ActionDescriptor.RelativePath);
        await using var stringWriter = new StringWriter();
        var view = new RazorView(engine, activator, new List<IRazorPage>(), page, HtmlEncoder.Default, new DiagnosticListener("ViewRenderService"));
        var viewContext = new ViewContext(actionContext, view, Reflection.GetPropertyValue(model, "ViewData"), Reflection.GetPropertyValue(model, "TempData"), stringWriter, new HtmlHelperOptions())
        {
            ExecutingFilePath = context.ActionDescriptor.RelativePath
        };
        var razorPage = (Page)page;
        razorPage.PageContext = model.PageContext;
        razorPage.ViewContext = viewContext;
        activator.Activate(razorPage, viewContext);
        await page.ExecuteAsync();
        return stringWriter.ToString();
    }

    /// <summary>
    /// Busca una página Razor utilizando el motor de vistas especificado.
    /// </summary>
    /// <param name="razorViewEngine">El motor de vistas Razor que se utilizará para buscar la página.</param>
    /// <param name="pageName">El nombre de la página Razor que se desea encontrar.</param>
    /// <returns>
    /// La instancia de <see cref="IRazorPage"/> correspondiente a la página encontrada.
    /// Si no se encuentra la página, se devolverá <c>null</c>.
    /// </returns>
    private IRazorPage FindPage(IRazorViewEngine razorViewEngine, string pageName)
    {
        var result = razorViewEngine.GetPage(null!, pageName);
        return result.Page;
    }

    /// <summary>
    /// Escribe contenido HTML en un archivo en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del archivo donde se escribirá el contenido HTML.</param>
    /// <param name="html">El contenido HTML que se escribirá en el archivo.</param>
    /// <returns>Una tarea que representa la operación asincrónica de escritura en el archivo.</returns>
    /// <remarks>
    /// Este método asegura que el directorio del archivo exista, creando el directorio si es necesario.
    /// Si la ruta del directorio es nula o vacía, el método no realizará ninguna acción.
    /// </remarks>
    private async Task WriteFile(string path, string html)
    {
        path = Util.Helpers.Web.GetPhysicalPath(path);
        var directory = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(directory))
            return;
        if (Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(path, html);
    }
}
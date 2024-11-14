using Util.Helpers;

namespace Util.Ui.Razor;

/// <summary>
/// Proporciona métodos estáticos para generar contenido HTML.
/// </summary>
public static class HtmlGenerator {
    /// <summary>
    /// Genera de manera asíncrona una lista de rutas de vistas a partir de los descriptores de acción de página.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de cadenas con las rutas de las vistas generadas.
    /// </returns>
    /// <remarks>
    /// Este método habilita la generación de HTML, obtiene los descriptores de acción de página y construye una lista de rutas
    /// a partir de esos descriptores. Luego, realiza una llamada HTTP asíncrona para cada ruta generada.
    /// </remarks>
    /// <seealso cref="GetPageActionDescriptors"/>
    /// <seealso cref="GetHost"/>
    /// <seealso cref="Web.Client.Get"/>
    public static async Task<List<string>> GenerateAsync( CancellationToken cancellationToken = default ) {
        EnableGenerateHtml();
        var result = new List<string>();
        var descriptors = GetPageActionDescriptors();
        foreach ( var descriptor in descriptors ) {
            var path = $"{GetHost()}/view{descriptor.ViewEnginePath}";
            result.Add( path );
            await Web.Client.Get( path ).GetResultAsync( cancellationToken );
        }
        return result.Distinct().ToList();
    }

    /// <summary>
    /// Habilita o deshabilita la generación de HTML.
    /// </summary>
    /// <param name="isGenerateHtml">Un valor booleano que indica si se debe generar HTML. El valor predeterminado es verdadero.</param>
    private static void EnableGenerateHtml( bool isGenerateHtml = true ) {
        var options = Ioc.Create<IOptions<RazorOptions>>();
        options.Value.IsGenerateHtml = isGenerateHtml;
    }

    /// <summary>
    /// Obtiene una lista de descriptores de acción de página.
    /// </summary>
    /// <returns>
    /// Una lista de <see cref="PageActionDescriptor"/> que representa los descriptores de acción de página.
    /// </returns>
    private static List<PageActionDescriptor> GetPageActionDescriptors() {
        var provider = Ioc.Create<IActionDescriptorCollectionProvider>();
        return provider.ActionDescriptors.Items.OfType<PageActionDescriptor>().ToList();
    }

    /// <summary>
    /// Obtiene la dirección del host actual en formato de URL.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la URL del host, incluyendo el esquema y el host.
    /// </returns>
    private static string GetHost() {
        return $"{Web.Request.Scheme}://{Web.Request.Host}";
    }
}
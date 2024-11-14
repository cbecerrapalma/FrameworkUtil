namespace Util.Templates.Razor.Filters; 

/// <summary>
/// Representa un filtro que permite la aplicación de operaciones asíncronas en plantillas.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ITemplateFilter"/> y proporciona 
/// funcionalidades específicas para filtrar datos de manera asíncrona.
/// </remarks>
public class PartialAsyncFilter : ITemplateFilter {
    /// <summary>
    /// Reemplaza las ocurrencias de "await Html.PartialAsync" por "Html.PartialAsync" en el texto proporcionado.
    /// </summary>
    /// <param name="template">El texto en el que se realizará el reemplazo.</param>
    /// <returns>El texto modificado con las ocurrencias reemplazadas.</returns>
    public string Filter( string template ) {
        return Util.Helpers.Regex.Replace( template, @"await\s+Html.PartialAsync", "Html.PartialAsync" ); 
    }
}
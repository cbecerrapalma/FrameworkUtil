namespace Util.Templates.Razor.Filters; 

/// <summary>
/// Representa un filtro que se puede aplicar a un modelo.
/// Implementa la interfaz <see cref="ITemplateFilter"/>.
/// </summary>
public class ModelFilter : ITemplateFilter {
    /// <summary>
    /// Reemplaza las declaraciones de modelo en una plantilla por la declaración de herencia correspondiente.
    /// </summary>
    /// <param name="template">La plantilla de texto que contiene declaraciones de modelo.</param>
    /// <returns>
    /// Una nueva cadena donde las declaraciones de modelo han sido reemplazadas por la declaración de herencia
    /// de <see cref="RazorEngineCore.RazorEngineTemplateBase{T}"/> con el tipo de modelo especificado.
    /// </returns>
    public string Filter( string template ) {
        return Util.Helpers.Regex.Replace( template, @"@model\s+(\w+)", "@inherits RazorEngineCore.RazorEngineTemplateBase<$1>" ); 
    }
}
namespace Util.Templates; 

/// <summary>
/// Define un contrato para filtros de plantillas.
/// </summary>
/// <remarks>
/// Esta interfaz permite implementar diferentes tipos de filtros que pueden ser aplicados a plantillas.
/// Los filtros pueden modificar o validar los datos antes de ser procesados por la plantilla.
/// </remarks>
public interface ITemplateFilter {
    /// <summary>
    /// Filtra una cadena de texto según un template especificado.
    /// </summary>
    /// <param name="template">La cadena que se utilizará como plantilla para filtrar.</param>
    /// <returns>Una nueva cadena que contiene los caracteres de la plantilla que se encuentran en la cadena original.</returns>
    /// <remarks>
    /// Este método toma como entrada un template y devuelve una cadena que solo incluye los caracteres
    /// presentes en el template. Si un carácter no está en el template, se omite en la cadena resultante.
    /// </remarks>
    /// <example>
    /// <code>
    /// string resultado = Filter("ejemplo");
    // resultado contendrá solo los caracteres que están en "ejemplo".
    /// </code>
    /// </example>
    string Filter( string template );
}
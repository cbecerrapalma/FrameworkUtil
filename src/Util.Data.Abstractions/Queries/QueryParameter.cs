using Util.Ui;

namespace Util.Data.Queries; 

/// <summary>
/// Representa un modelo para los parámetros de consulta.
/// </summary>
/// <remarks>
/// Este modelo se utiliza para definir los parámetros que se envían en una consulta.
/// </remarks>
/// <param name="queryParam">El nombre del parámetro de consulta.</param>
[Model( "queryParam" )]
public class QueryParameter : Pager {
    /// <summary>
    /// Representa una palabra clave utilizada en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar una palabra clave que puede ser 
    /// utilizada en diversas funcionalidades del sistema, como búsqueda o 
    /// categorización.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la palabra clave.
    /// </value>
    [Display( Name = "util.keyword" )]
    public string Keyword { get; set; }
}
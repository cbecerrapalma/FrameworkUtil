namespace Util.Reflections; 

/// <summary>
/// Define un contrato para encontrar ensamblados en una aplicación.
/// </summary>
public interface IAssemblyFinder {
    /// <summary>
    /// Obtiene o establece el patrón de omisión de ensamblados.
    /// </summary>
    /// <remarks>
    /// Este patrón se utiliza para especificar qué ensamblados deben ser ignorados 
    /// durante ciertas operaciones, como la carga de ensamblados o la reflexión.
    /// </remarks>
    /// <value>
    /// Un string que representa el patrón de omisión de ensamblados.
    /// </value>
    public string AssemblySkipPattern { get; set; }
    /// <summary>
    /// Busca y devuelve una lista de ensamblados (Assembly) disponibles.
    /// </summary>
    /// <returns>
    /// Una lista que contiene los ensamblados encontrados.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para explorar los ensamblados cargados en la aplicación,
    /// permitiendo realizar operaciones adicionales sobre ellos, como la reflexión o la 
    /// inspección de tipos.
    /// </remarks>
    /// <seealso cref="Assembly"/>
    List<Assembly> Find();
}
namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Interfaz que representa una cláusula SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlContent"/> y define la estructura básica
/// que deben seguir las cláusulas SQL en la implementación de consultas.
/// </remarks>
public interface ISqlClause : ISqlContent {
    /// <summary>
    /// Valida el estado actual del objeto.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto es válido; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para comprobar si el objeto cumple con las condiciones necesarias
    /// para ser considerado válido. Las condiciones específicas deben ser implementadas en la
    /// lógica del método.
    /// </remarks>
    bool Validate();
}
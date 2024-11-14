namespace Util.Data.Sql.Builders; 

/// <summary>
/// Interfaz que define un constructor de consultas SQL para verificar la existencia de registros.
/// </summary>
public interface IExistsSqlBuilder {
    /// <summary>
    /// Obtiene una cadena de texto que representa una consulta SQL.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene la consulta SQL.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para generar dinámicamente una consulta SQL 
    /// que puede ser utilizada para interactuar con una base de datos.
    /// </remarks>
    string GetSql();
}
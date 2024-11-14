using Util.Data.Sql.Builders.Caches;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa una caché de columnas para una base de datos SQL Server.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ColumnCacheBase"/> y proporciona funcionalidades específicas 
/// para manejar la caché de columnas en un entorno de SQL Server.
/// </remarks>
public class SqlServerColumnCache : ColumnCacheBase {
    private readonly ConcurrentDictionary<int, string> _cache;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerColumnCache"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece la instancia de dialecto de SQL Server y 
    /// inicializa el diccionario concurrente que se utilizará para almacenar 
    /// en caché las columnas.
    /// </remarks>
    private SqlServerColumnCache() : base( SqlServerDialect.Instance ) {
        _cache = new ConcurrentDictionary<int, string>();
    }

    public static readonly IColumnCache Instance = new SqlServerColumnCache();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de las columnas especificadas, normalizándolas si es necesario.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a normalizar.</param>
    /// <returns>Una cadena que contiene las columnas normalizadas y seguras.</returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar y recuperar columnas ya normalizadas, 
    /// evitando así cálculos innecesarios en llamadas posteriores con los mismos parámetros.
    /// </remarks>
    /// <inheritdoc />
    public override string GetSafeColumns( string columns ) {
        return _cache.GetOrAdd( columns.GetHashCode(), key => NormalizeColumns( columns ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de una columna dada, normalizando su nombre
    /// y almacenando el resultado en caché para mejorar el rendimiento en futuras
    /// solicitudes.
    /// </summary>
    /// <param name="column">El nombre de la columna que se desea normalizar.</param>
    /// <returns>Una cadena que representa el nombre normalizado de la columna.</returns>
    /// <remarks>
    /// Este método utiliza un mecanismo de caché para evitar la normalización
    /// repetida de columnas con el mismo nombre, lo que puede ser útil en
    /// escenarios donde se accede a las mismas columnas múltiples veces.
    /// </remarks>
    /// <seealso cref="NormalizeColumn(string)"/>
    public override string GetSafeColumn( string column ) {
        return _cache.GetOrAdd( column.GetHashCode(), key => NormalizeColumn( column ) );
    }
}
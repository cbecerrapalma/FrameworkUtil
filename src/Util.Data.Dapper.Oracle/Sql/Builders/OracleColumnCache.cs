using Util.Data.Sql.Builders.Caches;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa un caché de columnas específico para bases de datos Oracle.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ColumnCacheBase"/> y proporciona funcionalidades específicas
/// para manejar el caché de columnas en un contexto de base de datos Oracle.
/// </remarks>
public class OracleColumnCache : ColumnCacheBase {
    private readonly ConcurrentDictionary<int, string> _cache;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleColumnCache"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece la instancia de dialecto de Oracle y 
    /// crea un diccionario concurrente para almacenar en caché las columnas.
    /// </remarks>
    private OracleColumnCache() : base( OracleDialect.Instance ) {
        _cache = new ConcurrentDictionary<int, string>();
    }

    public static readonly IColumnCache Instance = new OracleColumnCache();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de las columnas especificadas.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a normalizar.</param>
    /// <returns>
    /// Una cadena que contiene las columnas normalizadas y seguras.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar y recuperar columnas normalizadas
    /// basándose en el código hash de la cadena de entrada, lo que mejora el rendimiento
    /// al evitar cálculos repetidos para las mismas columnas.
    /// </remarks>
    /// <seealso cref="NormalizeColumns(string)"/>
    public override string GetSafeColumns( string columns ) {
        return _cache.GetOrAdd( columns.GetHashCode(), key => NormalizeColumns( columns ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de una columna a partir de su nombre.
    /// </summary>
    /// <param name="column">El nombre de la columna que se desea normalizar.</param>
    /// <returns>Una cadena que representa el nombre normalizado de la columna.</returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar y recuperar nombres de columnas normalizados
    /// basándose en su código hash, lo que mejora el rendimiento al evitar cálculos repetidos
    /// para nombres de columna ya procesados.
    /// </remarks>
    /// <seealso cref="NormalizeColumn(string)"/>
    public override string GetSafeColumn( string column ) {
        return _cache.GetOrAdd( column.GetHashCode(), key => NormalizeColumn( column ) );
    }
}
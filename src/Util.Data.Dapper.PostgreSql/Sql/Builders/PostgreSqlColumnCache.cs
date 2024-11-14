using Util.Data.Sql.Builders.Caches;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa una caché de columnas específica para bases de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ColumnCacheBase"/> y proporciona funcionalidades específicas
/// para manejar la caché de columnas en un entorno de PostgreSQL.
/// </remarks>
public class PostgreSqlColumnCache : ColumnCacheBase {
    private readonly ConcurrentDictionary<int, string> _cache;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlColumnCache"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el dialecto de PostgreSQL y crea un diccionario concurrente 
    /// para almacenar en caché los nombres de las columnas.
    /// </remarks>
    private PostgreSqlColumnCache() : base( PostgreSqlDialect.Instance ) {
        _cache = new ConcurrentDictionary<int, string>();
    }

    public static readonly IColumnCache Instance = new PostgreSqlColumnCache();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación normalizada de las columnas de forma segura,
    /// utilizando un caché para evitar cálculos repetidos.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a normalizar.</param>
    /// <returns>
    /// Una cadena que contiene las columnas normalizadas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar los resultados de las normalizaciones
    /// basándose en el código hash de la cadena de entrada, lo que mejora el rendimiento
    /// al evitar cálculos innecesarios para entradas que ya han sido procesadas.
    /// </remarks>
    public override string GetSafeColumns( string columns ) {
        return _cache.GetOrAdd( columns.GetHashCode(), key => NormalizeColumns( columns ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de una columna a partir de su nombre.
    /// </summary>
    /// <param name="column">El nombre de la columna que se desea normalizar.</param>
    /// <returns>Una cadena que representa la columna normalizada y segura.</returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar y recuperar las columnas normalizadas
    /// basándose en el código hash del nombre de la columna, lo que mejora el rendimiento
    /// al evitar cálculos repetidos para nombres de columna ya procesados.
    /// </remarks>
    /// <seealso cref="NormalizeColumn(string)"/>
    public override string GetSafeColumn( string column ) {
        return _cache.GetOrAdd( column.GetHashCode(), key => NormalizeColumn( column ) );
    }
}
using Util.Data.Sql.Builders.Caches;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa un caché de columnas específico para bases de datos MySQL.
/// Esta clase hereda de <see cref="ColumnCacheBase"/> y proporciona
/// funcionalidades específicas para el manejo de columnas en un contexto
/// de base de datos MySQL.
/// </summary>
/// <remarks>
/// El caché de columnas permite optimizar el acceso y la gestión de
/// información sobre las columnas de las tablas en una base de datos
/// MySQL, mejorando así el rendimiento de las operaciones de lectura
/// y escritura.
/// </remarks>
public class MySqlColumnCache : ColumnCacheBase {
    private readonly ConcurrentDictionary<int, string> _cache;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlColumnCache"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el dialecto de MySQL y crea un diccionario concurrente 
    /// para almacenar en caché las columnas.
    /// </remarks>
    private MySqlColumnCache() : base( MySqlDialect.Instance ) {
        _cache = new ConcurrentDictionary<int, string>();
    }

    public static readonly IColumnCache Instance = new MySqlColumnCache();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una representación segura de las columnas especificadas.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a normalizar.</param>
    /// <returns>
    /// Una cadena que contiene las columnas normalizadas y seguras para su uso.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar los resultados de normalización basados en el código hash de la cadena de columnas.
    /// Si las columnas ya han sido normalizadas previamente, se recuperará el resultado del caché en lugar de volver a calcularlo.
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
    /// basándose en el código hash del nombre de la columna original. Si el nombre ya ha sido
    /// normalizado previamente, se recupera del caché; de lo contrario, se normaliza y se
    /// almacena en el caché.
    /// </remarks>
    /// <seealso cref="NormalizeColumn(string)"/>
    public override string GetSafeColumn( string column ) {
        return _cache.GetOrAdd( column.GetHashCode(), key => NormalizeColumn( column ) );
    }
}
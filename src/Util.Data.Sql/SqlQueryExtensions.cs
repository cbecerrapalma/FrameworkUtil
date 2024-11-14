using Util.Data.Sql.Database;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para realizar consultas SQL.
/// </summary>
public static partial class SqlQueryExtensions
{

    #region GetConnection(Obtener conexión a la base de datos.)

    /// <summary>
    /// Obtiene una conexión a la base de datos a partir de una fuente de consulta SQL.
    /// </summary>
    /// <param name="source">La fuente de consulta SQL de la cual se obtendrá la conexión.</param>
    /// <returns>
    /// Un objeto <see cref="IDbConnection"/> que representa la conexión a la base de datos,
    /// o <c>null</c> si la fuente no es un administrador de conexiones.
    /// </returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y verifica si la fuente
    /// es una instancia de <see cref="IConnectionManager"/>. Si es así, se obtiene la conexión
    /// a través del administrador de conexiones. De lo contrario, se devuelve <c>null</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    public static IDbConnection GetConnection(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        if (source is IConnectionManager manager)
            return manager.GetConnection();
        return null;
    }

    #endregion

    #region SetConnection(Configurar la conexión a la base de datos.)

    /// <summary>
    /// Establece la conexión para la consulta SQL especificada.
    /// </summary>
    /// <param name="source">La consulta SQL en la que se establecerá la conexión.</param>
    /// <param name="connection">La conexión a la base de datos que se asignará a la consulta.</param>
    /// <returns>La consulta SQL con la conexión establecida.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y permite asignar una conexión a la consulta.
    /// Si la consulta es un gestor de conexiones, se llama al método <see cref="IConnectionManager.SetConnection(IDbConnection)"/>.
    /// </remarks>
    public static ISqlQuery SetConnection(this ISqlQuery source, IDbConnection connection)
    {
        source.CheckNull(nameof(source));
        if (source is IConnectionManager manager)
            manager.SetConnection(connection);
        return source;
    }

    #endregion

    #region Clear(Limpieza)

    /// <summary>
    /// Limpia la consulta SQL actual.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a limpiar.</param>
    /// <returns>La misma consulta SQL después de haber sido limpiada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método se utiliza para restablecer el estado de la consulta SQL, eliminando cualquier 
    /// condición o cláusula previamente establecida. Es útil cuando se desea reutilizar una instancia 
    /// de consulta sin los parámetros anteriores.
    /// </remarks>
    public static ISqlQuery Clear(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        source.SqlBuilder.Clear();
        return source;
    }

    #endregion

    #region NewSqlBuilder(Crear un nuevo generador SQL)

    /// <summary>
    /// Crea una nueva instancia de un constructor SQL a partir de una consulta SQL existente.
    /// </summary>
    /// <param name="source">La consulta SQL de la cual se generará el nuevo constructor SQL.</param>
    /// <returns>Una nueva instancia de <see cref="ISqlBuilder"/> asociada a la consulta SQL proporcionada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión que permite a los desarrolladores crear un nuevo constructor SQL 
    /// de manera fluida a partir de una consulta SQL existente, asegurando que la consulta no sea nula 
    /// antes de proceder con la creación del constructor.
    /// </remarks>
    public static ISqlBuilder NewSqlBuilder(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return source.SqlBuilder.New();
    }

    #endregion
}
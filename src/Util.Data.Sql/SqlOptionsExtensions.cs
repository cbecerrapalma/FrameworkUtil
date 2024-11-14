namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para configurar opciones de SQL.
/// </summary>
public static class SqlOptionsExtensions
{

    #region ConnectionString(Configurar la cadena de conexión de la base de datos.)

    /// <summary>
    /// Establece la cadena de conexión para las opciones de SQL.
    /// </summary>
    /// <param name="options">Las opciones de SQL a las que se les asignará la cadena de conexión.</param>
    /// <param name="connectionString">La cadena de conexión que se asignará a las opciones.</param>
    /// <returns>Las opciones de SQL actualizadas con la nueva cadena de conexión.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es nulo.</exception>
    public static SqlOptions ConnectionString(this SqlOptions options, string connectionString)
    {
        options.CheckNull(nameof(options));
        options.ConnectionString = connectionString;
        return options;
    }

    #endregion

    #region Connection(Configurar la conexión a la base de datos.)

    /// <summary>
    /// Establece la conexión de base de datos para las opciones de SQL.
    /// </summary>
    /// <param name="options">Las opciones de SQL a las que se les asignará la conexión.</param>
    /// <param name="connection">La conexión de base de datos que se asignará a las opciones.</param>
    /// <returns>Las opciones de SQL actualizadas con la conexión asignada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es nulo.</exception>
    public static SqlOptions Connection(this SqlOptions options, IDbConnection connection)
    {
        options.CheckNull(nameof(options));
        options.Connection = connection;
        return options;
    }

    #endregion

    #region LogCategory(Configurar la categoría de registro.)

    /// <summary>
    /// Establece la categoría de registro para las opciones de SQL.
    /// </summary>
    /// <param name="options">Las opciones de SQL a las que se aplicará la categoría de registro.</param>
    /// <param name="logCategory">La categoría de registro que se desea establecer.</param>
    /// <returns>Las opciones de SQL actualizadas con la nueva categoría de registro.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es nulo.</exception>
    public static SqlOptions LogCategory(this SqlOptions options, string logCategory)
    {
        options.CheckNull(nameof(options));
        options.LogCategory = logCategory;
        return options;
    }

    #endregion
}
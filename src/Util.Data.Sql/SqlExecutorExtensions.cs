using Util.Data.Sql.Database;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para ejecutar comandos SQL.
/// </summary>
public static class SqlExecutorExtensions
{

    #region GetConnection(Obtener conexión a la base de datos.)

    /// <summary>
    /// Obtiene una conexión a la base de datos a partir de un ejecutor SQL.
    /// </summary>
    /// <param name="source">El ejecutor SQL que se utilizará para obtener la conexión.</param>
    /// <returns>
    /// Una instancia de <see cref="IDbConnection"/> si el <paramref name="source"/> es un <see cref="IConnectionManager"/>; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="ISqlExecutor"/> para facilitar la obtención de conexiones a la base de datos.
    /// </remarks>
    public static IDbConnection GetConnection(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        if (source is IConnectionManager manager)
            return manager.GetConnection();
        return null;
    }

    #endregion

    #region SetConnection(Configurar la conexión a la base de datos.)

    /// <summary>
    /// Establece una conexión a un ejecutor SQL.
    /// </summary>
    /// <param name="source">El ejecutor SQL al que se le establecerá la conexión.</param>
    /// <param name="connection">La conexión a la base de datos que se va a establecer.</param>
    /// <returns>El ejecutor SQL con la conexión establecida.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="ISqlExecutor"/> permitiendo establecer una conexión 
    /// a través de un objeto que implementa <see cref="IConnectionManager"/> si es aplicable.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="IConnectionManager"/>
    public static ISqlExecutor SetConnection(this ISqlExecutor source, IDbConnection connection)
    {
        source.CheckNull(nameof(source));
        if (source is IConnectionManager manager)
            manager.SetConnection(connection);
        return source;
    }

    #endregion

    #region GetTransaction(Obtener transacciones de la base de datos.)

    /// <summary>
    /// Obtiene una transacción de la fuente proporcionada.
    /// </summary>
    /// <param name="source">La fuente que implementa <see cref="ISqlExecutor"/>.</param>
    /// <returns>
    /// Una instancia de <see cref="IDbTransaction"/> si la fuente es un <see cref="ITransactionManager"/>; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la fuente es nula y lanza una excepción si es así.
    /// Si la fuente es un <see cref="ITransactionManager"/>, se llama al método <see cref="ITransactionManager.GetTransaction"/> para obtener la transacción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    public static IDbTransaction GetTransaction(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            return manager.GetTransaction();
        return null;
    }

    #endregion

    #region SetTransaction(Configurar transacciones de base de datos.)

    /// <summary>
    /// Establece una transacción para el ejecutor SQL especificado.
    /// </summary>
    /// <param name="source">El ejecutor SQL en el que se establecerá la transacción.</param>
    /// <param name="transaction">La transacción que se va a establecer.</param>
    /// <returns>El ejecutor SQL con la transacción establecida.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="ISqlExecutor"/> permitiendo asociar una transacción 
    /// a un ejecutor SQL, siempre que el ejecutor sea también un <see cref="ITransactionManager"/>.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="ITransactionManager"/>
    public static ISqlExecutor SetTransaction(this ISqlExecutor source, IDbTransaction transaction)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            manager.SetTransaction(transaction);
        return source;
    }

    #endregion

    #region BeginTransaction(Iniciar transacción)

    /// <summary>
    /// Inicia una nueva transacción en el contexto del ejecutor SQL proporcionado.
    /// </summary>
    /// <param name="source">El ejecutor SQL que se utilizará para iniciar la transacción.</param>
    /// <returns>
    /// Un objeto <see cref="IDbTransaction"/> que representa la transacción iniciada, 
    /// o <c>null</c> si el ejecutor no es un administrador de transacciones.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es de tipo <see cref="ITransactionManager"/> 
    /// antes de intentar iniciar una transacción. Si no lo es, se devuelve <c>null</c>.
    /// </remarks>
    public static IDbTransaction BeginTransaction(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            return manager.BeginTransaction();
        return null;
    }

    /// <summary>
    /// Inicia una nueva transacción en el contexto del ejecutor SQL especificado.
    /// </summary>
    /// <param name="source">El ejecutor SQL que inicia la transacción.</param>
    /// <param name="isolationLevel">El nivel de aislamiento que se aplicará a la transacción.</param>
    /// <returns>
    /// Un objeto <see cref="IDbTransaction"/> que representa la transacción iniciada, 
    /// o <c>null</c> si el ejecutor SQL no es un administrador de transacciones.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el <paramref name="source"/> es nulo y lanza una excepción 
    /// si es así. Si el <paramref name="source"/> implementa <see cref="ITransactionManager"/>, 
    /// se llama al método <see cref="ITransactionManager.BeginTransaction(IsolationLevel)"/> 
    /// para iniciar la transacción con el nivel de aislamiento especificado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static IDbTransaction BeginTransaction(this ISqlExecutor source, IsolationLevel isolationLevel)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            return manager.BeginTransaction(isolationLevel);
        return null;
    }

    #endregion

    #region CommitTransaction(Enviar transacción)

    /// <summary>
    /// Confirma la transacción actual en el ejecutor SQL proporcionado.
    /// </summary>
    /// <param name="source">El ejecutor SQL que se utilizará para confirmar la transacción.</param>
    /// <returns>El mismo ejecutor SQL después de confirmar la transacción.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método verifica si el ejecutor SQL implementa la interfaz <see cref="ITransactionManager"/> 
    /// y, de ser así, llama al método <see cref="ITransactionManager.CommitTransaction"/> 
    /// para confirmar la transacción actual.
    /// </remarks>
    public static ISqlExecutor CommitTransaction(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            manager.CommitTransaction();
        return source;
    }

    #endregion

    #region RollbackTransaction(Revertir transacción)

    /// <summary>
    /// Revierte la transacción actual si el ejecutor de SQL es un administrador de transacciones.
    /// </summary>
    /// <param name="source">El ejecutor de SQL que se utilizará para revertir la transacción.</param>
    /// <returns>El mismo ejecutor de SQL después de intentar revertir la transacción.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método verifica si el <paramref name="source"/> implementa la interfaz <see cref="ITransactionManager"/>.
    /// Si es así, se llama al método <see cref="ITransactionManager.RollbackTransaction"/> para revertir la transacción.
    /// </remarks>
    public static ISqlExecutor RollbackTransaction(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        if (source is ITransactionManager manager)
            manager.RollbackTransaction();
        return source;
    }

    #endregion

    #region Clear(Limpiar)

    /// <summary>
    /// Limpia el constructor de SQL del ejecutor SQL proporcionado.
    /// </summary>
    /// <param name="source">El ejecutor SQL que se va a limpiar.</param>
    /// <returns>El mismo ejecutor SQL después de haber sido limpiado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ISqlExecutor"/> 
    /// que permite reiniciar el estado del constructor de SQL, eliminando 
    /// cualquier comando SQL previamente configurado.
    /// </remarks>
    public static ISqlExecutor Clear(this ISqlExecutor source)
    {
        source.CheckNull(nameof(source));
        source.SqlBuilder.Clear();
        return source;
    }

    #endregion
}
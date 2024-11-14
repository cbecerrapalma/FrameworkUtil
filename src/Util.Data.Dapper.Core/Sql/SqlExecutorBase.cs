using Util.Data.Sql;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Clase base abstracta para la ejecución de consultas SQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación común para la ejecución de consultas SQL,
/// y debe ser heredada por clases concretas que implementen la lógica específica de ejecución.
/// </remarks>
public abstract class SqlExecutorBase : SqlQueryBase, ISqlExecutor
{

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlExecutorBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración para la ejecución de SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para las operaciones de SQL.</param>
    protected SqlExecutorBase(IServiceProvider serviceProvider, SqlOptions options, IDatabase database) : base(serviceProvider, options, database)
    {
    }

    #endregion

    #region ExecuteAsync(Ejecutar operaciones de agregar, eliminar y modificar.)

    /// <summary>
    /// Ejecuta de forma asíncrona una operación que devuelve un entero.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en milisegundos para esperar la ejecución. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un entero que representa el resultado de la operación ejecutada.
    /// </returns>
    /// <remarks>
    /// Este método realiza una serie de pasos antes y después de la ejecución de la operación, 
    /// incluyendo la verificación de condiciones previas y el manejo de transacciones. 
    /// En caso de que ocurra una excepción, se revertirá la transacción activa.
    /// </remarks>
    /// <exception cref="Exception">
    /// Se lanza una excepción si ocurre un error durante la ejecución de la operación.
    /// </exception>
    public virtual async Task<int> ExecuteAsync(int? timeout = null)
    {
        int result = 0;
        try
        {
            if (ExecuteBefore() == false)
                return 0;
            var connection = GetConnection();
            result = await connection.ExecuteAsync(GetSql(), Params, GetTransaction(), timeout);
            return result;
        }
        catch (Exception)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteProcedureAsync(Ejecutar operaciones de inserción, eliminación y modificación de procedimientos almacenados.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado de manera asíncrona.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se desea ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <returns>
    /// Un entero que representa el resultado de la ejecución del procedimiento almacenado.
    /// </returns>
    /// <remarks>
    /// Este método realiza la ejecución de un procedimiento almacenado en la base de datos.
    /// Si la ejecución previa no es exitosa, se devuelve el valor predeterminado.
    /// En caso de que ocurra una excepción, se realiza un rollback de la transacción.
    /// Al finalizar, se ejecutan las acciones definidas en el método <c>ExecuteAfter</c>.
    /// </remarks>
    /// <exception cref="Exception">
    /// Se lanza una excepción si ocurre un error durante la ejecución del procedimiento.
    /// </exception>
    public async Task<int> ExecuteProcedureAsync(string procedure, int? timeout = null)
    {
        int result = 0;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            SetSql(GetProcedure(procedure));
            var connection = GetConnection();
            result = await connection.ExecuteAsync(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType());
            return result;
        }
        catch (Exception)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion
}
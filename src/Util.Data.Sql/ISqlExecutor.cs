using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql; 

/// <summary>
/// Define un contrato para la ejecución de consultas SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlQuery"/> y <see cref="ISqlOperation"/>,
/// proporcionando métodos para ejecutar operaciones SQL y realizar consultas.
/// </remarks>
public interface ISqlExecutor : ISqlQuery, ISqlOperation {
    /// <summary>
    /// Ejecuta una tarea de forma asíncrona y devuelve un entero como resultado.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en milisegundos que se permitirá para la ejecución de la tarea. Si es null, no se aplicará límite de tiempo.</param>
    /// <returns>Un objeto <see cref="Task{int}"/> que representa la tarea asíncrona. El resultado de la tarea es un entero.</returns>
    /// <remarks>
    /// Esta función puede ser utilizada para ejecutar operaciones que pueden tardar un tiempo indeterminado.
    /// Si se proporciona un valor para el parámetro <paramref name="timeout"/>, la tarea se cancelará si no se completa dentro de ese tiempo.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task<int> ExecuteAsync( int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la finalización del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor entero que indica el resultado de la ejecución del procedimiento.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados en una base de datos de manera asíncrona, 
    /// lo que puede mejorar la capacidad de respuesta de la aplicación al evitar bloqueos en el hilo principal.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="procedure"/> es nulo o vacío.</exception>
    /// <exception cref="SqlException">Se lanza si ocurre un error durante la ejecución del procedimiento almacenado.</exception>
    Task<int> ExecuteProcedureAsync( string procedure, int? timeout = null );
}
namespace Util.Data.Sql.Database; 

/// <summary>
/// Interfaz que define las operaciones básicas para la gestión de transacciones.
/// </summary>
public interface ITransactionManager {
    /// <summary>
    /// Establece la transacción de base de datos que se utilizará.
    /// </summary>
    /// <param name="transaction">La transacción de base de datos que se asignará.</param>
    /// <remarks>
    /// Este método permite definir una transacción específica que se utilizará para las operaciones de base de datos subsiguientes.
    /// Asegúrese de que la transacción proporcionada esté activa y no haya sido completada o revertida.
    /// </remarks>
    void SetTransaction(IDbTransaction transaction);
    /// <summary>
    /// Obtiene la transacción actual de la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto que representa la transacción de la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método es útil para realizar operaciones de base de datos que requieren
    /// un contexto de transacción, asegurando que las operaciones se completen de manera
    /// atómica.
    /// </remarks>
    /// <seealso cref="IDbTransaction"/>
    IDbTransaction GetTransaction();
    /// <summary>
    /// Inicia una nueva transacción en la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDbTransaction"/> que representa la transacción activa.
    /// </returns>
    /// <remarks>
    /// Este método permite comenzar una transacción que puede ser utilizada para agrupar varias operaciones de base de datos.
    /// Asegúrese de llamar a <see cref="IDbTransaction.Commit"/> o <see cref="IDbTransaction.Rollback"/> 
    /// para finalizar la transacción correctamente.
    /// </remarks>
    IDbTransaction BeginTransaction();
    /// <summary>
    /// Inicia una nueva transacción con el nivel de aislamiento especificado.
    /// </summary>
    /// <param name="isolationLevel">El nivel de aislamiento que se aplicará a la transacción.</param>
    /// <returns>Una instancia de <see cref="IDbTransaction"/> que representa la transacción iniciada.</returns>
    /// <remarks>
    /// Los niveles de aislamiento controlan cómo las transacciones interactúan entre sí y cómo los cambios en la base de datos son visibles para otras transacciones.
    /// Asegúrese de elegir un nivel de aislamiento que se ajuste a los requisitos de concurrencia y consistencia de su aplicación.
    /// </remarks>
    /// <seealso cref="IDbTransaction"/>
    /// <seealso cref="IsolationLevel"/>
    IDbTransaction BeginTransaction( IsolationLevel isolationLevel );
    /// <summary>
    /// Confirma y aplica todas las operaciones realizadas en la transacción actual.
    /// </summary>
    /// <remarks>
    /// Este método debe ser llamado para finalizar la transacción y asegurar que todos los cambios
    /// realizados sean persistidos en la base de datos. Si no se llama a este método, los cambios
    /// pueden perderse o revertirse.
    /// </remarks>
    void CommitTransaction();
    /// <summary>
    /// Revierte la transacción actual, deshaciendo todos los cambios realizados desde el último punto de confirmación.
    /// </summary>
    /// <remarks>
    /// Este método es útil en situaciones donde se necesita deshacer cambios debido a un error o una condición inesperada.
    /// Asegúrese de que se haya iniciado una transacción antes de llamar a este método, de lo contrario, puede generar una excepción.
    /// </remarks>
    void RollbackTransaction();
}
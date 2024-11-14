using Util.Dependency;

namespace Util.Data; 

/// <summary>
/// Interfaz que define un gestor de acciones para el patrón Unit of Work.
/// </summary>
/// <remarks>
/// Esta interfaz extiende <see cref="IScopeDependency"/> y se utiliza para manejar
/// las acciones que se deben realizar dentro de una unidad de trabajo.
/// </remarks>
public interface IUnitOfWorkActionManager : IScopeDependency {
    /// <summary>
    /// Registra una acción asíncrona que se ejecutará posteriormente.
    /// </summary>
    /// <param name="action">Una función que representa la acción asíncrona a registrar.</param>
    /// <remarks>
    /// Esta función permite registrar acciones que se ejecutarán en un contexto asíncrono.
    /// Asegúrese de que la acción proporcionada sea adecuada para su uso en un entorno asíncrono.
    /// </remarks>
    void Register(Func<Task> action);
    /// <summary>
    /// Ejecuta una tarea de manera asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método permite realizar operaciones que pueden requerir tiempo, 
    /// como llamadas a servicios externos o procesamiento de datos, 
    /// sin bloquear el hilo de ejecución actual.
    /// </remarks>
    /// <returns>
    /// Un <see cref="Task"/> que representa la operación asíncrona.
    /// </returns>
    /// <exception cref="Exception">
    /// Se lanza si ocurre un error durante la ejecución de la tarea.
    /// </exception>
    Task ExecuteAsync();
}
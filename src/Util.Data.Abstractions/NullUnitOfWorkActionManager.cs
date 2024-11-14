namespace Util.Data;

/// <summary>
/// Clase que implementa la interfaz <see cref="IUnitOfWorkActionManager"/> 
/// y representa un gestor de acciones de unidad de trabajo nulo.
/// </summary>
public class NullUnitOfWorkActionManager : IUnitOfWorkActionManager {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NullUnitOfWorkActionManager"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza como un marcador de posición para la gestión de acciones en una unidad de trabajo nula.
    /// No realiza ninguna acción y se utiliza principalmente en situaciones donde se requiere una implementación de 
    /// <see cref="IUnitOfWorkActionManager"/> pero no se desea realizar ninguna operación.
    /// </remarks>
    public NullUnitOfWorkActionManager() {
    }

    public static readonly IUnitOfWorkActionManager Instance = new NullUnitOfWorkActionManager();

    /// <inheritdoc />
    /// <summary>
    /// Registra una acción asíncrona que se ejecutará posteriormente.
    /// </summary>
    /// <param name="action">La acción asíncrona a registrar, que se ejecutará como un <see cref="Task"/>.</param>
    /// <remarks>
    /// Este método permite agregar una función que se puede ejecutar de manera asíncrona. 
    /// Asegúrese de que la acción registrada maneje adecuadamente cualquier excepción que pueda ocurrir durante su ejecución.
    /// </remarks>
    public void Register( Func<Task> action ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta de manera asíncrona una tarea.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. 
    /// El valor devuelto es una tarea completada de forma inmediata.
    /// </returns>
    /// <remarks>
    /// Este método no realiza ninguna acción y simplemente completa la tarea.
    /// </remarks>
    public Task ExecuteAsync() {
        return Task.CompletedTask;
    }
}
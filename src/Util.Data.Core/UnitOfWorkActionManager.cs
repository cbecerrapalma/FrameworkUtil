using Util.Dependency;

namespace Util.Data;

/// <summary>
/// Clase que representa un contenedor de inversión de control (IoC).
/// </summary>
/// <remarks>
/// Esta clase permite gestionar la creación y la resolución de dependencias
/// en una aplicación, facilitando la implementación del patrón de diseño 
/// Inversión de Control.
/// </remarks>
/// <param name="id">Identificador único para la instancia del contenedor.</param>
[Ioc( 1 )]
public class UnitOfWorkActionManager : IUnitOfWorkActionManager {
    private readonly List<Func<Task>> _actions;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWorkActionManager"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de acciones que se ejecutarán como parte de la unidad de trabajo.
    /// </remarks>
    public UnitOfWorkActionManager() {
        _actions = new List<Func<Task>>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una acción asíncrona para su ejecución posterior.
    /// </summary>
    /// <param name="action">La acción asíncrona a registrar. Si es <c>null</c>, no se realiza ninguna acción.</param>
    public void Register( Func<Task> action ) {
        if ( action == null )
            return;
        _actions.Add( action );
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta de manera asíncrona todas las acciones almacenadas en la colección.
    /// </summary>
    /// <remarks>
    /// Este método itera a través de todas las acciones y las ejecuta de forma asíncrona. 
    /// Una vez que todas las acciones han sido ejecutadas, la colección de acciones se limpia.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    public async Task ExecuteAsync() {
        foreach ( var action in _actions )
            await action();
        _actions.Clear();
    }
}
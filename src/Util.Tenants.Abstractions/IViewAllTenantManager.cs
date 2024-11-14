namespace Util.Tenants;

/// <summary>
/// Interfaz que define un contrato para la gestión de todos los inquilinos.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/>, lo que indica que las implementaciones de esta interfaz 
/// deben ser creadas como dependencias transitorias.
/// </remarks>
public interface IViewAllTenantManager : ITransientDependency {
    /// <summary>
    /// Indica si el filtro de inquilinos está deshabilitado.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de inquilinos está deshabilitado; de lo contrario, <c>false</c>.
    /// </returns>
    bool IsDisableTenantFilter();
    /// <summary>
    /// Habilita la vista de todos los elementos de forma asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método permite a los usuarios acceder a todos los elementos disponibles en el sistema.
    /// Se debe llamar a este método cuando se requiera mostrar una lista completa de elementos.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea indica el éxito o fracaso de la operación.
    /// </returns>
    Task EnableViewAllAsync();
    /// <summary>
    /// Desactiva la vista de todos los elementos de manera asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método puede ser utilizado para restringir el acceso a la visualización de todos los elementos
    /// en una interfaz de usuario o en un sistema de gestión de datos. 
    /// Se recomienda llamar a este método en situaciones donde se necesite limitar la visibilidad de los datos.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de desactivación. 
    /// El resultado de la tarea no contiene información adicional.
    /// </returns>
    Task DisableViewAllAsync();
}
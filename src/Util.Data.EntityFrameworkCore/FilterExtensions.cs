namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Proporciona métodos de extensión para filtrar colecciones.
/// </summary>
public static class FilterExtensions {
    /// <summary>
    /// Habilita el filtro de eliminación en la operación de filtro especificada.
    /// </summary>
    /// <param name="source">La operación de filtro en la que se habilitará el filtro de eliminación.</param>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="IFilterOperation"/> para permitir la habilitación
    /// del filtro de eliminación, lo que permite que las operaciones de filtrado consideren los elementos
    /// que han sido marcados para eliminación.
    /// </remarks>
    public static void EnableDeleteFilter( this IFilterOperation source ) {
        source.EnableFilter<IDelete>();
    }

    /// <summary>
    /// Desactiva el filtro de eliminación en la operación de filtro especificada.
    /// </summary>
    /// <param name="source">La operación de filtro en la que se desactivará el filtro de eliminación.</param>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que se puede utilizar para volver a habilitar el filtro de eliminación cuando ya no sea necesario.
    /// </returns>
    /// <remarks>
    /// Este método permite desactivar temporalmente el filtro de eliminación, lo que puede ser útil en situaciones donde se requiere acceder a datos que normalmente estarían excluidos por este filtro.
    /// </remarks>
    /// <seealso cref="IFilterOperation"/>
    /// <seealso cref="IDelete"/>
    public static IDisposable DisableDeleteFilter( this IFilterOperation source ) {
        return source.DisableFilter<IDelete>();
    }

    /// <summary>
    /// Habilita el filtro de inquilino en la operación de filtro especificada.
    /// </summary>
    /// <param name="source">La operación de filtro en la que se habilitará el filtro de inquilino.</param>
    public static void EnableTenantFilter( this IFilterOperation source ) {
        source.EnableFilter<ITenant>();
    }

    /// <summary>
    /// Desactiva el filtro de inquilino en la operación de filtro especificada.
    /// </summary>
    /// <param name="source">La operación de filtro en la que se desactivará el filtro de inquilino.</param>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que se puede utilizar para volver a habilitar el filtro de inquilino.
    /// </returns>
    /// <remarks>
    /// Este método es útil cuando se necesita realizar operaciones que no deben estar restringidas por el filtro de inquilino.
    /// Asegúrese de llamar al método <see cref="IDisposable.Dispose"/> en el objeto devuelto para restaurar el estado original.
    /// </remarks>
    public static IDisposable DisableTenantFilter( this IFilterOperation source ) {
        return source.DisableFilter<ITenant>();
    }
}
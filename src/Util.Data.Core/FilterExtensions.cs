using Util.Data.Filters;

namespace Util.Data; 

/// <summary>
/// Proporciona métodos de extensión para filtrar colecciones.
/// </summary>
public static class FilterExtensions {
    /// <summary>
    /// Habilita un filtro en la operación de filtro especificada si es del tipo adecuado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea habilitar. Debe ser una clase.</typeparam>
    /// <param name="source">La operación de filtro en la que se habilitará el filtro.</param>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IFilterOperation"/> para permitir la habilitación de filtros 
    /// específicos basados en el tipo proporcionado. Si la operación de filtro es de tipo <see cref="IFilterSwitch"/>,
    /// se llamará al método correspondiente para habilitar el filtro.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IFilterOperation"/>
    /// <seealso cref="IFilterSwitch"/>
    public static void EnableFilter<TFilterType>( this IFilterOperation source ) where TFilterType : class {
        source.CheckNull( nameof( source ) );
        if( source is IFilterSwitch filterSwitch )
            filterSwitch.EnableFilter<TFilterType>();
    }

    /// <summary>
    /// Desactiva un filtro del tipo especificado en la operación de filtro.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea desactivar. Debe ser una clase.</typeparam>
    /// <param name="source">La operación de filtro en la que se desactivará el filtro.</param>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que se puede utilizar para restaurar el estado del filtro a su configuración original.
    /// </returns>
    /// <remarks>
    /// Si la operación de filtro no es un <see cref="IFilterSwitch"/>, se devolverá <see cref="DisposeAction.Null"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IFilterOperation"/>
    /// <seealso cref="IFilterSwitch"/>
    /// <seealso cref="DisposeAction"/>
    public static IDisposable DisableFilter<TFilterType>( this IFilterOperation source ) where TFilterType : class {
        source.CheckNull( nameof( source ) );
        if( source is IFilterSwitch filterSwitch )
            return filterSwitch.DisableFilter<TFilterType>();
        return DisposeAction.Null;
    }
}
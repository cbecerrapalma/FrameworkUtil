namespace Util.Data.Filters; 

/// <summary>
/// Define una interfaz para un interruptor de filtro.
/// </summary>
public interface IFilterSwitch {
    /// <summary>
    /// Habilita un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se va a habilitar. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Este método permite activar un filtro genérico en el contexto actual. 
    /// Asegúrese de que el tipo proporcionado cumpla con los requisitos necesarios 
    /// para su uso en el sistema.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el filtro no se puede habilitar debido a una condición no válida.
    /// </exception>
    /// <seealso cref="DisableFilter{TFilterType}"/>
    void EnableFilter<TFilterType>() where TFilterType : class;
    /// <summary>
    /// Desactiva un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea desactivar. Debe ser una clase.</typeparam>
    /// <returns>Un objeto que implementa <see cref="IDisposable"/> para gestionar la desactivación del filtro.</returns>
    /// <remarks>
    /// Este método permite desactivar filtros de manera segura y eficiente. 
    /// Asegúrese de llamar al método <see cref="IDisposable.Dispose"/> 
    /// en el objeto devuelto para liberar los recursos asociados con la desactivación del filtro.
    /// </remarks>
    /// <exception cref="ArgumentException">Se lanza si el tipo especificado no es válido.</exception>
    /// <seealso cref="IDisposable"/>
    IDisposable DisableFilter<TFilterType>() where TFilterType : class;
}
using Util.Dependency;

namespace Util.Data.Filters; 

/// <summary>
/// Define un filtro que puede ser utilizado en el contexto de inyección de dependencias.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para implementar filtros que pueden ser aplicados a diversas operaciones
/// dentro de la aplicación. Al heredar de <see cref="ITransientDependency"/>, se asegura que cada
/// instancia del filtro sea transitoria, es decir, se crea una nueva instancia cada vez que se solicita.
/// </remarks>
public interface IFilter : ITransientDependency {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    bool IsEnabled { get; }
    /// <summary>
    /// Determina si la entidad especificada está habilitada.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si la entidad está habilitada; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para comprobar el estado de habilitación de una entidad genérica
    /// en el contexto de la aplicación. Asegúrese de que la entidad esté correctamente configurada
    /// antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IsEntityDisabled{TEntity}"/>
    bool IsEntityEnabled<TEntity>();
    /// <summary>
    /// Habilita la funcionalidad del componente.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para activar todas las características y comportamientos del componente.
    /// Asegúrese de que el componente esté correctamente inicializado antes de llamar a este método.
    /// </remarks>
    void Enable();
    /// <summary>
    /// Desactiva el objeto actual y libera los recursos asociados.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="IDisposable"/> que representa el estado desactivado.
    /// </returns>
    /// <remarks>
    /// Este método debe ser llamado cuando el objeto ya no sea necesario, 
    /// para asegurar que todos los recursos se liberen adecuadamente.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    IDisposable Disable();
    /// <summary>
    /// Obtiene una expresión que representa un predicado para filtrar entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad para el cual se genera la expresión.</typeparam>
    /// <param name="state">Un objeto que contiene el estado o los parámetros necesarios para construir la expresión.</param>
    /// <returns>
    /// Una expresión que representa un predicado que puede ser utilizado para filtrar entidades de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para construir dinámicamente consultas basadas en el estado proporcionado.
    /// </remarks>
    /// <seealso cref="Func{TEntity, bool}"/>
    Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class;
}

/// <summary>
/// Define una interfaz genérica para filtros que extiende la interfaz base <see cref="IFilter"/>.
/// </summary>
/// <typeparam name="TFilterType">El tipo de filtro que debe ser una clase.</typeparam>
public interface IFilter<TFilterType> : IFilter where TFilterType : class {
}
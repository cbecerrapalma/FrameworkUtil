using Util.Dependency;

namespace Util.Data.Filters; 

/// <summary>
/// Interfaz que define un administrador de filtros.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IFilterSwitch"/> y <see cref="IScopeDependency"/>,
/// lo que permite que los implementadores gestionen filtros y dependencias de ámbito.
/// </remarks>
public interface IFilterManager : IFilterSwitch,IScopeDependency {
    /// <summary>
    /// Obtiene un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea obtener. Debe ser una clase.</typeparam>
    /// <returns>Una instancia de <see cref="IFilter"/> que corresponde al tipo de filtro solicitado.</returns>
    /// <remarks>
    /// Este método permite obtener filtros de manera genérica, proporcionando flexibilidad al trabajar con diferentes tipos de filtros.
    /// Asegúrese de que el tipo especificado cumpla con las restricciones necesarias para su uso.
    /// </remarks>
    /// <seealso cref="IFilter"/>
    IFilter GetFilter<TFilterType>() where TFilterType : class;
    /// <summary>
    /// Obtiene un filtro del tipo especificado.
    /// </summary>
    /// <param name="filterType">El tipo de filtro que se desea obtener.</param>
    /// <returns>Una instancia de <see cref="IFilter"/> que corresponde al tipo de filtro solicitado.</returns>
    /// <remarks>
    /// Este método permite obtener filtros de diferentes tipos en función de la necesidad del contexto.
    /// Asegúrese de que el tipo de filtro proporcionado implementa la interfaz <see cref="IFilter"/>.
    /// </remarks>
    /// <seealso cref="IFilter"/>
    IFilter GetFilter( Type filterType );
    /// <summary>
    /// Determina si la entidad especificada está habilitada.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si la entidad está habilitada; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para verificar el estado de habilitación de diferentes tipos de entidades
    /// en el contexto de la aplicación. Asegúrese de que la entidad esté correctamente configurada antes de
    /// llamar a este método.
    /// </remarks>
    /// <seealso cref="IsEntityDisabled{TEntity}"/>
    bool IsEntityEnabled<TEntity>();
    /// <summary>
    /// Determina si un filtro de tipo específico está habilitado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de tipo <typeparamref name="TFilterType"/> está habilitado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una restricción de tipo genérico para asegurar que solo se pueda utilizar con tipos de clase.
    /// </remarks>
    /// <seealso cref="IsEnabled{TFilterType}"/>
    bool IsEnabled<TFilterType>() where TFilterType : class;
    /// <summary>
    /// Obtiene una expresión que representa una condición para filtrar entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se va a filtrar.</typeparam>
    /// <param name="state">Un objeto que contiene el estado o los criterios utilizados para construir la expresión.</param>
    /// <returns>Una expresión que evalúa a verdadero o falso para las entidades del tipo especificado.</returns>
    /// <remarks>
    /// Esta función permite construir dinámicamente una expresión de filtrado basada en el estado proporcionado.
    /// Es útil en escenarios donde se requiere aplicar condiciones de filtrado en consultas LINQ.
    /// </remarks>
    /// <seealso cref="System.Linq.Expressions.Expression{TDelegate}"/>
    Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class;
}
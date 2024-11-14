using Util.Data.Queries;
using Util.Data.Queries.Conditions;

namespace Util.Data;

/// <summary>
/// Proporciona métodos de extensión para trabajar con objetos de tipo <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{

    #region Where(Agregar objeto de condiciones de consulta.)

    /// <summary>
    /// Filtra una secuencia de entidades de tipo <typeparamref name="TEntity"/> 
    /// utilizando una condición especificada.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de las entidades en la secuencia.</typeparam>
    /// <param name="source">La secuencia de entidades a filtrar.</param>
    /// <param name="condition">La condición que se aplicará para filtrar las entidades.</param>
    /// <returns>
    /// Una nueva secuencia que contiene las entidades que cumplen con la condición especificada.
    /// Si la condición es <c>null</c>, se devuelve la secuencia original sin cambios.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="source"/> o <paramref name="condition"/> son <c>null</c>.
    /// </exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IQueryable{T}"/> 
    /// permitiendo aplicar condiciones dinámicas a las consultas.
    /// </remarks>
    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, ICondition<TEntity> condition) where TEntity : class
    {
        source.CheckNull(nameof(source));
        condition.CheckNull(nameof(condition));
        var predicate = condition.GetCondition();
        if (predicate == null)
            return source;
        return source.Where(predicate);
    }

    #endregion

    #region WhereIf(Agregar condiciones de consulta según las reglas.)

    /// <summary>
    /// Filtra una secuencia de valores de tipo <typeparamref name="TEntity"/> 
    /// utilizando una expresión de predicado si se cumple una condición.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de los elementos en la secuencia.</typeparam>
    /// <param name="source">La secuencia de elementos sobre la que se aplicará el filtro.</param>
    /// <param name="predicate">La expresión que define el predicado para el filtrado.</param>
    /// <param name="condition">Una condición que determina si se debe aplicar el filtro.</param>
    /// <returns>
    /// Una nueva secuencia que contiene los elementos filtrados si <paramref name="condition"/> es verdadero; 
    /// de lo contrario, la secuencia original sin cambios.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IQueryable{T}"/>
    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, bool condition) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return condition ? source.Where(predicate) : source;
    }

    #endregion

    #region WhereIfNotEmpty(Agregar condiciones de consulta según las reglas.)

    /// <summary>
    /// Filtra una secuencia de entidades según una condición proporcionada, 
    /// solo si la condición no está vacía.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está filtrando.</typeparam>
    /// <param name="source">La secuencia de entidades sobre la que se aplicará el filtro.</param>
    /// <param name="condition">La expresión que representa la condición de filtrado.</param>
    /// <returns>
    /// Una nueva secuencia de entidades que cumplen con la condición especificada, 
    /// o la secuencia original si la condición está vacía.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es útil para aplicar condiciones de filtrado de manera condicional, 
    /// evitando la necesidad de comprobar explícitamente si la condición está vacía 
    /// antes de aplicar el filtro.
    /// </remarks>
    /// <seealso cref="IQueryable{T}"/>
    public static IQueryable<TEntity> WhereIfNotEmpty<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> condition) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return source.Where(new WhereIfNotEmptyCondition<TEntity>(condition));
    }

    #endregion

    #region OrIfNotEmpty(Agregar condiciones de consulta OR.)

    /// <summary>
    /// Filtra una secuencia de entidades de tipo <typeparamref name="TEntity"/> aplicando condiciones lógicas OR.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de las entidades en la secuencia.</typeparam>
    /// <param name="source">La secuencia de entidades sobre la cual se aplicarán las condiciones.</param>
    /// <param name="condition1">La primera condición que se evaluará.</param>
    /// <param name="condition2">La segunda condición que se evaluará.</param>
    /// <param name="conditions">Condiciones adicionales que se evaluarán en la secuencia.</param>
    /// <returns>
    /// Una nueva secuencia de entidades que cumplen al menos una de las condiciones especificadas.
    /// </returns>
    /// <remarks>
    /// Este método permite combinar múltiples condiciones utilizando la lógica OR. Si alguna de las condiciones es verdadera,
    /// la entidad será incluida en el resultado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IQueryable{T}"/>
    public static IQueryable<TEntity> OrIfNotEmpty<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> condition1,
        Expression<Func<TEntity, bool>> condition2, params Expression<Func<TEntity, bool>>[] conditions) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return source.Where(new OrIfNotEmptyCondition<TEntity>(condition1, condition2, conditions));
    }

    #endregion

    #region Between(Agregar condiciones de consulta de rango.)

    /// <summary>
    /// Filtra una secuencia de entidades de tipo <typeparamref name="TEntity"/> 
    /// para incluir solo aquellos elementos cuyo valor de la propiedad especificada 
    /// se encuentra dentro de un rango definido por los parámetros <paramref name="min"/> 
    /// y <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está filtrando.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <param name="source">La secuencia de entidades que se va a filtrar.</param>
    /// <param name="propertyExpression">Una expresión que representa la propiedad que se va a evaluar.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica si los límites del rango son inclusivos o exclusivos.</param>
    /// <returns>
    /// Una secuencia de entidades que cumplen con la condición de estar dentro del rango 
    /// definido por <paramref name="min"/> y <paramref name="max"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método utiliza una condición de segmento de enteros para filtrar los elementos 
    /// de la secuencia. Se puede utilizar para realizar consultas más específicas en bases de datos 
    /// que implementan IQueryable.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    public static IQueryable<TEntity> Between<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> propertyExpression, int? min, int? max, Boundary boundary = Boundary.Both) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return source.Where(new IntSegmentCondition<TEntity, TProperty>(propertyExpression, min, max, boundary));
    }

    /// <summary>
    /// Filtra una secuencia de entidades para incluir solo aquellas cuyos valores de la propiedad especificada 
    /// se encuentran dentro de un rango definido por los parámetros mínimo y máximo.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <param name="source">La secuencia de entidades que se va a filtrar.</param>
    /// <param name="propertyExpression">Una expresión que representa la propiedad a evaluar.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica si los límites del rango son inclusivos o exclusivos.</param>
    /// <returns>
    /// Una secuencia de entidades que cumplen con la condición de estar dentro del rango especificado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para realizar consultas que requieren filtrar datos numéricos dentro de un rango específico.
    /// </remarks>
    /// <seealso cref="DoubleSegmentCondition{TEntity, TProperty}"/>
    public static IQueryable<TEntity> Between<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> propertyExpression, double? min, double? max, Boundary boundary = Boundary.Both) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return source.Where(new DoubleSegmentCondition<TEntity, TProperty>(propertyExpression, min, max, boundary));
    }

    /// <summary>
    /// Filtra una colección de entidades para incluir solo aquellas cuyos valores de una propiedad específica se encuentran dentro de un rango determinado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <param name="source">La fuente de datos que se va a filtrar.</param>
    /// <param name="propertyExpression">Una expresión que representa la propiedad a evaluar.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica si los límites del rango son inclusivos o exclusivos. Por defecto es <see cref="Boundary.Both"/>.</param>
    /// <returns>
    /// Una consulta que contiene las entidades que cumplen con la condición del rango especificado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para realizar filtrados en consultas LINQ donde se necesita limitar los resultados a un rango específico de valores.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    public static IQueryable<TEntity> Between<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> propertyExpression, decimal? min, decimal? max, Boundary boundary = Boundary.Both) where TEntity : class
    {
        source.CheckNull(nameof(source));
        return source.Where(new DecimalSegmentCondition<TEntity, TProperty>(propertyExpression, min, max, boundary));
    }

    /// <summary>
    /// Filtra una secuencia de entidades basándose en un rango de fechas especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está filtrando.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <param name="source">La secuencia de entidades a filtrar.</param>
    /// <param name="propertyExpression">Una expresión que representa la propiedad de la entidad que contiene la fecha a evaluar.</param>
    /// <param name="min">La fecha mínima del rango. Puede ser nula.</param>
    /// <param name="max">La fecha máxima del rango. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la hora en la comparación de fechas. Por defecto es verdadero.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango (inferior y superior).</param>
    /// <returns>
    /// Una secuencia de entidades que cumplen con el criterio de filtrado basado en el rango de fechas.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método permite filtrar entidades en función de un rango de fechas, 
    /// ya sea considerando solo la fecha o incluyendo la hora, según el parámetro <paramref name="includeTime"/>.
    /// </remarks>
    /// <seealso cref="DateTimeSegmentCondition{TEntity, TProperty}"/>
    /// <seealso cref="DateSegmentCondition{TEntity, TProperty}"/>
    public static IQueryable<TEntity> Between<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max, bool includeTime = true, Boundary boundary = Boundary.Both) where TEntity : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (includeTime)
            return source.Where(new DateTimeSegmentCondition<TEntity, TProperty>(propertyExpression, min, max, boundary));
        return source.Where(new DateSegmentCondition<TEntity, TProperty>(propertyExpression, min, max, boundary));
    }

    #endregion

    #region OrderBy(Ordenar)

    /// <summary>
    /// Ordena una colección de entidades de tipo <typeparamref name="TEntity"/> 
    /// según los parámetros especificados.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se va a ordenar.</typeparam>
    /// <param name="source">La colección de entidades que se va a ordenar.</param>
    /// <param name="parameter">Los parámetros de paginación que contienen la información de ordenación.</param>
    /// <param name="defaultOrder">El orden predeterminado a aplicar si no se especifica otro.</param>
    /// <returns>
    /// Una colección ordenada de entidades de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IQueryable{T}"/> 
    /// para permitir el ordenamiento dinámico basado en los parámetros proporcionados.
    /// Si el parámetro de orden está vacío, se devuelve la colección original sin cambios.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="source"/> o <paramref name="parameter"/> son nulos.
    /// </exception>
    /// <seealso cref="IQueryable{T}"/>
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, IPage parameter, string defaultOrder = null) where TEntity : class
    {
        source.CheckNull(nameof(source));
        parameter.CheckNull(nameof(parameter));
        InitOrder(source, parameter, defaultOrder);
        return parameter.Order.IsEmpty() ? source : source.OrderBy(parameter.Order);
    }

    /// <summary>
    /// Inicializa el orden de una consulta si no se ha especificado uno.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <param name="source">La fuente de datos que se va a ordenar.</param>
    /// <param name="parameter">Los parámetros de paginación que contienen el orden.</param>
    /// <param name="defaultOrder">El orden predeterminado que se aplicará si no se ha especificado otro.</param>
    /// <remarks>
    /// Este método verifica si ya se ha establecido un orden en los parámetros. 
    /// Si no se ha especificado un orden y la expresión de la consulta no contiene 
    /// ninguna cláusula de ordenamiento, se asigna el orden predeterminado.
    /// </remarks>
    /// <seealso cref="IQueryable{T}"/>
    /// <seealso cref="IPage"/>
    private static void InitOrder<TEntity>(IQueryable<TEntity> source, IPage parameter, string defaultOrder)
    {
        if (parameter.Order.IsEmpty() == false)
            return;
        var expression = source.Expression.SafeString();
        if (expression.Contains(".OrderBy(") || expression.Contains(".OrderByDescending("))
            return;
        if (defaultOrder.IsEmpty())
            return;
        parameter.Order = defaultOrder;
    }

    #endregion
}
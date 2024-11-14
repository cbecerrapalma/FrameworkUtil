using Util.Data.Queries;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql; 

/// <summary>
/// Proporciona métodos de extensión para construir cláusulas WHERE en consultas.
/// </summary>
public static class WhereClauseExtensions {

    #region Where

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa la interfaz <see cref="IWhere"/> 
    /// para agregar una cláusula WHERE a una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente al que se le aplicará la cláusula WHERE.</param>
    /// <param name="condition">La condición que se utilizará en la cláusula WHERE.</param>
    /// <returns>El objeto fuente con la cláusula WHERE aplicada.</returns>
    /// <remarks>
    /// Este método verifica si el objeto fuente es nulo y lanza una excepción si es así.
    /// Si el objeto fuente implementa <see cref="ISqlPartAccessor"/>, se agrega la condición 
    /// a la cláusula WHERE existente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static T Where<T>( this T source, ISqlCondition condition ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.And( condition );
        return source;
    }

    /// <summary>
    /// Filtra el objeto fuente basado en una condición especificada en la columna dada.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente en el que se aplicará el filtro.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <returns>El objeto fuente después de aplicar el filtro.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IWhere"/> 
    /// permitiendo agregar condiciones a la cláusula WHERE de una consulta SQL.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto fuente es nulo.</exception>
    public static T Where<T>( this T source, string column, object value, Operator @operator = Operator.Equal ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, value, @operator );
        return source;
    }

    /// <summary>
    /// Filtra el objeto fuente basado en una condición especificada en la columna dada.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente, que debe implementar la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se aplicará el filtro.</param>
    /// <param name="column">El nombre de la columna que se utilizará para la condición del filtro.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la cláusula SQL.</param>
    /// <param name="operator">El operador que se utilizará en la condición del filtro. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <returns>El objeto fuente después de aplicar el filtro.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IWhere"/> 
    /// permitiendo agregar condiciones de filtrado a la cláusula WHERE de una consulta SQL.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto fuente es nulo.</exception>
    public static T Where<T>( this T source, string column, ISqlBuilder builder, Operator @operator = Operator.Equal ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, builder, @operator );
        return source;
    }

    /// <summary>
    /// Agrega una cláusula WHERE a la consulta SQL basada en el objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente al que se le añadirá la cláusula WHERE.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="action">Una acción que permite construir la cláusula SQL utilizando un <see cref="ISqlBuilder"/>.</param>
    /// <param name="operator">El operador que se utilizará en la cláusula WHERE. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <returns>El objeto fuente con la cláusula WHERE añadida.</returns>
    /// <remarks>
    /// Este método permite construir consultas SQL dinámicamente al agregar condiciones a la cláusula WHERE.
    /// Asegúrese de que el objeto fuente no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto fuente es nulo.</exception>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="Operator"/>
    public static T Where<T>( this T source, string column, Action<ISqlBuilder> action, Operator @operator = Operator.Equal ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, action, @operator );
        return source;
    }

    #endregion

    #region In

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa la interfaz <see cref="IWhere"/> 
    /// para agregar una cláusula WHERE que verifica si el valor de una columna está dentro de un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se aplicará la cláusula WHERE.</param>
    /// <param name="column">El nombre de la columna que se evaluará.</param>
    /// <param name="values">Una colección de valores que se utilizarán para la comparación.</param>
    /// <returns>El objeto fuente con la cláusula WHERE aplicada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es útil para construir consultas SQL dinámicamente, permitiendo filtrar resultados 
    /// basados en una lista de valores específicos.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T In<T>( this T source, string column, IEnumerable values ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, values,Operator.In );
        return source;
    }

    /// <summary>
    /// Extensión que agrega una cláusula WHERE con un operador IN a una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se aplicará la cláusula WHERE.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplicará el operador IN.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utiliza para construir la consulta SQL.</param>
    /// <returns>La fuente original con la cláusula WHERE aplicada.</returns>
    /// <remarks>
    /// Este método es útil para construir consultas SQL dinámicas donde se necesita filtrar resultados basados en múltiples valores.
    /// Asegúrese de que la fuente no sea nula antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static T In<T>( this T source, string column, ISqlBuilder builder ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, builder, Operator.In );
        return source;
    }

    /// <summary>
    /// Extensión que permite agregar una cláusula WHERE con un operador IN a una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto que representa la consulta SQL actual.</param>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará el operador IN.</param>
    /// <param name="action">Una acción que permite construir la cláusula IN utilizando un <see cref="ISqlBuilder"/>.</param>
    /// <returns>El objeto <paramref name="source"/> con la cláusula WHERE actualizada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Esta extensión es útil para construir consultas SQL de manera fluida, permitiendo agregar condiciones complejas
    /// de forma legible y mantenible.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T In<T>( this T source, string column, Action<ISqlBuilder> action ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, action, Operator.In );
        return source;
    }

    #endregion

    #region NotIn

    /// <summary>
    /// Agrega una cláusula WHERE que verifica que el valor de una columna no esté en un conjunto de valores especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se aplica la cláusula WHERE.</param>
    /// <param name="column">El nombre de la columna que se va a evaluar.</param>
    /// <param name="values">Una colección de valores que se compararán con el valor de la columna.</param>
    /// <returns>El objeto original <paramref name="source"/> con la cláusula WHERE aplicada.</returns>
    /// <remarks>
    /// Este método es una extensión que permite construir consultas SQL de manera fluida.
    /// Es necesario que el objeto <paramref name="source"/> no sea nulo y que implemente <see cref="IWhere"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T NotIn<T>( this T source, string column, IEnumerable values ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, values, Operator.NotIn );
        return source;
    }

    /// <summary>
    /// Agrega una cláusula WHERE que verifica que el valor de la columna especificada no esté en un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo de la instancia que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La instancia sobre la que se aplica la cláusula WHERE.</param>
    /// <param name="column">El nombre de la columna que se va a evaluar.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utiliza para construir la consulta SQL.</param>
    /// <returns>La instancia original con la cláusula WHERE aplicada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite agregar de manera fluida una cláusula WHERE a una consulta SQL.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T NotIn<T>( this T source, string column, ISqlBuilder builder ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, builder, Operator.NotIn );
        return source;
    }

    /// <summary>
    /// Agrega una cláusula "Not In" a la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente sobre la cual se aplicará la cláusula "Not In".</param>
    /// <param name="column">El nombre de la columna en la que se aplicará la condición.</param>
    /// <param name="action">Una acción que permite construir la lista de valores que no deben estar en la columna especificada.</param>
    /// <returns>La fuente original con la cláusula "Not In" aplicada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de las consultas SQL al permitir filtrar resultados que no se encuentran en una lista específica.
    /// Asegúrese de que la fuente no sea nula antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static T NotIn<T>( this T source, string column, Action<ISqlBuilder> action ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Where( column, action, Operator.NotIn );
        return source;
    }

    #endregion

    #region IsNull

    /// <summary>
    /// Verifica si el valor de la columna especificada es nulo en la cláusula WHERE.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto en el que se realiza la verificación de nulidad.</param>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <returns>
    /// El objeto original <paramref name="source"/> después de aplicar la verificación de nulidad.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T IsNull<T>( this T source, string column ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.IsNull( column );
        return source;
    }

    #endregion

    #region IsNotNull

    /// <summary>
    /// Verifica que la columna especificada no sea nula en la cláusula WHERE de una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto en el que se está realizando la verificación.</param>
    /// <param name="column">El nombre de la columna que se está verificando.</param>
    /// <returns>El objeto original <paramref name="source"/> después de aplicar la verificación.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="IWhere"/> 
    /// para agregar una condición que verifica que la columna especificada no sea nula.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T IsNotNull<T>( this T source, string column ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.IsNotNull( column );
        return source;
    }

    #endregion

    #region IsEmpty

    /// <summary>
    /// Verifica si una columna específica en la cláusula WHERE está vacía.
    /// </summary>
    /// <typeparam name="T">El tipo que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La instancia del objeto que se está verificando.</param>
    /// <param name="column">El nombre de la columna que se desea verificar.</param>
    /// <returns>Devuelve la instancia original de <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IWhere"/> 
    /// al permitir la verificación de columnas vacías en la cláusula WHERE.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T IsEmpty<T>( this T source, string column ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.IsEmpty( column );
        return source;
    }

    #endregion

    #region IsNotEmpty

    /// <summary>
    /// Verifica que la columna especificada no esté vacía en la cláusula WHERE.
    /// </summary>
    /// <typeparam name="T">El tipo que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La instancia de la que se está llamando al método.</param>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <returns>
    /// La instancia original de <typeparamref name="T"/> para permitir la encadenación de métodos.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión que permite agregar una condición a la cláusula WHERE
    /// para asegurarse de que el valor de la columna especificada no esté vacío.
    /// </remarks>
    public static T IsNotEmpty<T>( this T source, string column ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.IsNotEmpty( column );
        return source;
    }

    #endregion

    #region Between

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa <see cref="IWhere"/> para agregar una cláusula 
    /// "BETWEEN" a la consulta SQL, permitiendo filtrar resultados dentro de un rango especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se aplica la extensión.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplica el filtro.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica si el rango incluye los límites. Por defecto es <see cref="Boundary.Both"/>.</param>
    /// <returns>El objeto original con la cláusula "BETWEEN" añadida a su consulta.</returns>
    /// <remarks>
    /// Este método permite construir consultas SQL más complejas al facilitar la inclusión de filtros 
    /// basados en rangos de valores. Es útil en escenarios donde se requiere filtrar datos numéricos 
    /// o de fecha dentro de un rango específico.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="Boundary"/>
    public static T Between<T>( this T source, string column, int? min, int? max, Boundary boundary = Boundary.Both ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Between( column, min, max, boundary );
        return source;
    }

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa <see cref="IWhere"/> 
    /// para agregar una cláusula "BETWEEN" a la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se aplica la extensión.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplica la cláusula "BETWEEN".</param>
    /// <param name="min">El valor mínimo del rango (incluido o excluido según el límite).</param>
    /// <param name="max">El valor máximo del rango (incluido o excluido según el límite).</param>
    /// <param name="boundary">Especifica si los límites son inclusivos o exclusivos.</param>
    /// <returns>El objeto original con la cláusula "BETWEEN" aplicada.</returns>
    /// <remarks>
    /// Esta función permite filtrar resultados en una consulta SQL basándose en un rango de valores.
    /// Se puede especificar si los límites del rango son inclusivos o exclusivos mediante el parámetro <paramref name="boundary"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="Boundary"/>
    public static T Between<T>( this T source, string column, double? min, double? max, Boundary boundary = Boundary.Both ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Between( column, min, max, boundary );
        return source;
    }

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa <see cref="IWhere"/> para agregar una cláusula "BETWEEN" 
    /// en una consulta SQL, permitiendo filtrar resultados dentro de un rango específico.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto en el que se aplica la extensión.</param>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la cláusula "BETWEEN".</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica si los límites del rango son inclusivos o exclusivos. Por defecto es <see cref="Boundary.Both"/>.</param>
    /// <returns>El objeto original <paramref name="source"/> después de aplicar la cláusula "BETWEEN".</returns>
    /// <remarks>
    /// Este método permite construir consultas SQL de manera más fluida y legible, facilitando la inclusión de 
    /// condiciones de rango en las consultas.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="Boundary"/>
    public static T Between<T>( this T source, string column, decimal? min, decimal? max, Boundary boundary = Boundary.Both ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Between( column, min, max, boundary );
        return source;
    }

    /// <summary>
    /// Filtra los registros de la fuente especificada que se encuentran entre dos fechas.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente de datos sobre la que se aplicará el filtro.</param>
    /// <param name="column">El nombre de la columna que se utilizará para la comparación de fechas.</param>
    /// <param name="min">La fecha mínima del rango. Puede ser nula.</param>
    /// <param name="max">La fecha máxima del rango. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la hora en la comparación. Por defecto es <c>true</c>.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango. Puede ser nulo.</param>
    /// <returns>La fuente de datos filtrada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente de datos permitiendo aplicar un filtro basado en un rango de fechas.
    /// Si la fuente no es nula y es un <see cref="ISqlPartAccessor"/>, se aplicará la cláusula WHERE correspondiente.
    /// </remarks>
    public static T Between<T>( this T source, string column, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = null ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Between( column, min, max, includeTime, boundary );
        return source;
    }

    #endregion

    #region Exists

    /// <summary>
    /// Verifica si existe una cláusula WHERE en el objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto fuente en el que se verifica la existencia de la cláusula WHERE.</param>
    /// <param name="builder">El constructor SQL que se utilizará para construir la consulta.</param>
    /// <returns>
    /// El objeto fuente, después de verificar la cláusula WHERE.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IWhere"/> 
    /// para permitir la verificación de la existencia de una cláusula WHERE en la consulta SQL.
    /// </remarks>
    public static T Exists<T>( this T source, ISqlBuilder builder ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Exists( builder );
        return source;
    }

    /// <summary>
    /// Verifica si existe una cláusula en la consulta SQL actual.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto en el que se aplica la verificación.</param>
    /// <param name="action">La acción que se ejecutará para construir la cláusula SQL.</param>
    /// <returns>El objeto original <paramref name="source"/> después de aplicar la verificación.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="IWhere"/> 
    /// permitiendo agregar una cláusula EXISTS a la consulta SQL.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Exists<T>( this T source, Action<ISqlBuilder> action ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Exists( action );
        return source;
    }

    #endregion

    #region NotExists

    /// <summary>
    /// Extensión que agrega una cláusula "Not Exists" a una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se aplicará la cláusula "Not Exists".</param>
    /// <param name="builder">El constructor SQL que se utilizará para generar la consulta.</param>
    /// <returns>El objeto fuente con la cláusula "Not Exists" aplicada.</returns>
    /// <remarks>
    /// Este método verifica que la fuente no sea nula y, si es un <see cref="ISqlPartAccessor"/>,
    /// añade la cláusula "Not Exists" al objeto <see cref="ISqlBuilder"/> proporcionado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T NotExists<T>( this T source, ISqlBuilder builder ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.NotExists( builder );
        return source;
    }

    /// <summary>
    /// Extensión que permite agregar una cláusula NOT EXISTS a una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se aplicará la cláusula NOT EXISTS.</param>
    /// <param name="action">La acción que define la subconsulta para la cláusula NOT EXISTS.</param>
    /// <returns>Retorna la fuente original con la cláusula NOT EXISTS aplicada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Esta extensión es útil para construir consultas SQL que requieren verificar la no existencia de registros
    /// en una subconsulta. Asegúrese de que la fuente implementa la interfaz <see cref="IWhere"/> 
    /// para que la extensión funcione correctamente.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T NotExists<T>( this T source, Action<ISqlBuilder> action ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if ( source is ISqlPartAccessor accessor )
            accessor.WhereClause.NotExists( action );
        return source;
    }

    #endregion

    #region AppendWhere

    /// <summary>
    /// Agrega una cláusula WHERE a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que debe implementar la interfaz <see cref="IWhere"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula WHERE.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula WHERE a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar.</param>
    /// <returns>La fuente original con la cláusula WHERE agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IWhere"/> 
    /// permitiendo agregar dinámicamente cláusulas WHERE a las consultas SQL.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendWhere<T>( this T source, string sql, bool raw = false ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.AppendSql( sql, raw );
        return source;
    }

    #endregion

    #region ClearWhere

    /// <summary>
    /// Limpia la cláusula WHERE de un objeto que implementa la interfaz <see cref="IWhere"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IWhere"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula WHERE.</param>
    /// <returns>El objeto original después de limpiar la cláusula WHERE.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite a cualquier objeto que implemente la interfaz <see cref="IWhere"/> 
    /// limpiar su cláusula WHERE de manera sencilla. Si el objeto también implementa <see cref="ISqlPartAccessor"/>, 
    /// se accede a la propiedad <see cref="ISqlPartAccessor.WhereClause"/> para realizar la limpieza.
    /// </remarks>
    /// <seealso cref="IWhere"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearWhere<T>( this T source ) where T : IWhere {
        source.CheckNull( nameof( source ) );
        if( source is ISqlPartAccessor accessor )
            accessor.WhereClause.Clear();
        return source;
    }

    #endregion
}
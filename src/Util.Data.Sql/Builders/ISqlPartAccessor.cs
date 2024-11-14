using Util.Data.Sql.Builders.Clauses;
using Util.Data.Sql.Builders.Params;
using Util.Data.Sql.Builders.Sets;

namespace Util.Data.Sql.Builders; 

/// <summary>
/// Define una interfaz para acceder a partes de una consulta SQL.
/// </summary>
public interface ISqlPartAccessor {
    /// <summary>
    /// Obtiene el dialecto utilizado por la implementación actual.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IDialect"/> que representa el dialecto.
    /// </value>
    IDialect Dialect { get; }
    /// <summary>
    /// Obtiene la instancia del administrador de parámetros.
    /// </summary>
    /// <value>
    /// Una implementación de <see cref="IParameterManager"/> que gestiona los parámetros.
    /// </value>
    IParameterManager ParameterManager { get; }
    /// <summary>
    /// Obtiene la cláusula de inicio asociada.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IStartClause"/> que representa la cláusula de inicio.
    /// </value>
    IStartClause StartClause { get; }
    /// <summary>
    /// Obtiene la cláusula de selección asociada.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="ISelectClause"/> que representa la cláusula de selección.
    /// </value>
    ISelectClause SelectClause { get; }
    /// <summary>
    /// Representa una cláusula "FROM" en una consulta.
    /// </summary>
    /// <remarks>
    /// Esta interfaz permite acceder a la cláusula "FROM" de una consulta, 
    /// que especifica las fuentes de datos de las que se obtendrán los resultados.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IFromClause"/> 
    /// que representa la cláusula "FROM".
    /// </value>
    IFromClause FromClause { get; }
    /// <summary>
    /// Obtiene la cláusula de unión asociada.
    /// </summary>
    /// <value>
    /// La cláusula de unión que implementa la interfaz <see cref="IJoinClause"/>.
    /// </value>
    IJoinClause JoinClause { get; }
    /// <summary>
    /// Obtiene la cláusula WHERE utilizada en una consulta.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a la cláusula WHERE 
    /// que se aplica a la consulta actual. La cláusula WHERE se utiliza para 
    /// filtrar los resultados de la consulta según criterios específicos.
    /// </remarks>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IWhereClause"/> que representa 
    /// la cláusula WHERE de la consulta.
    /// </returns>
    IWhereClause WhereClause { get; }
    /// <summary>
    /// Obtiene la cláusula GROUP BY asociada a la consulta.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IGroupByClause"/> que representa la cláusula GROUP BY.
    /// </value>
    IGroupByClause GroupByClause { get; }
    /// <summary>
    /// Obtiene la cláusula de ordenamiento utilizada en la consulta.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IOrderByClause"/> que representa la cláusula de ordenamiento.
    /// </value>
    IOrderByClause OrderByClause { get; }
    /// <summary>
    /// Obtiene la cláusula de inserción asociada.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IInsertClause"/> que representa la cláusula de inserción.
    /// </value>
    IInsertClause InsertClause { get; }
    /// <summary>
    /// Obtiene la cláusula final de la expresión.
    /// </summary>
    /// <value>
    /// La cláusula final que implementa la interfaz <see cref="IEndClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula final de una expresión, 
    /// que puede ser utilizada para definir el comportamiento de finalización 
    /// en una consulta o expresión específica.
    /// </remarks>
    IEndClause EndClause { get; }
    /// <summary>
    /// Obtiene la instancia de <see cref="ISqlBuilderSet"/> asociada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a la configuración del generador de SQL,
    /// facilitando la construcción de consultas SQL de manera fluida y estructurada.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="ISqlBuilderSet"/> que representa el conjunto de 
    /// configuraciones para la construcción de SQL.
    /// </returns>
    ISqlBuilderSet SqlBuilderSet { get; }
}
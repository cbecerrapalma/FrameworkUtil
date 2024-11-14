using Util.Data.Queries;

namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Interfaz que representa una cláusula WHERE en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y se utiliza para definir
/// las condiciones que deben cumplirse para filtrar los resultados de una consulta.
/// </remarks>
public interface IWhereClause : ISqlClause {
    /// <summary>
    /// Combina la condición especificada con una operación lógica "Y".
    /// </summary>
    /// <param name="condition">La condición SQL que se desea combinar.</param>
    /// <remarks>
    /// Este método permite agregar una condición adicional a la consulta SQL actual, 
    /// utilizando la operación lógica "Y". Esto significa que ambas condiciones deben 
    /// cumplirse para que el resultado sea verdadero.
    /// </remarks>
    /// <seealso cref="ISqlCondition"/>
    void And( ISqlCondition condition );
    /// <summary>
    /// Agrega una condición lógica "OR" a la consulta SQL.
    /// </summary>
    /// <param name="condition">La condición que se debe agregar a la consulta.</param>
    /// <remarks>
    /// Este método permite combinar múltiples condiciones utilizando el operador lógico "OR". 
    /// Se puede utilizar para construir consultas más complejas donde se requiere que al menos una 
    /// de las condiciones sea verdadera.
    /// </remarks>
    /// <seealso cref="And(ISqlCondition)"/>
    void Or( ISqlCondition condition );
    /// <summary>
    /// Filtra los registros según el valor de una columna específica y un operador dado.
    /// </summary>
    /// <param name="column">El nombre de la columna en la que se aplicará el filtro.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="operator">El operador que se utilizará para la comparación.</param>
    /// <remarks>
    /// Este método permite construir consultas dinámicas basadas en condiciones específicas.
    /// Asegúrese de que el valor y el operador sean compatibles con el tipo de datos de la columna.
    /// </remarks>
    /// <seealso cref="Operator"/>
    void Where( string column, object value, Operator @operator );
    /// <summary>
    /// Filtra los resultados de una consulta SQL en función de una columna específica y un operador dado.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se aplicará el filtro.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utiliza para construir la consulta SQL.</param>
    /// <param name="operator">El operador que se utilizará para comparar los valores en la columna especificada.</param>
    /// <remarks>
    /// Este método permite construir condiciones de filtrado dinámicamente en una consulta SQL, facilitando la creación de consultas más complejas.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Where( string column, ISqlBuilder builder, Operator @operator );
    /// <summary>
    /// Filtra los resultados de una consulta SQL basándose en una columna específica y una acción definida por el usuario.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará el filtro.</param>
    /// <param name="action">Una acción que recibe un objeto <see cref="ISqlBuilder"/> para construir la cláusula SQL.</param>
    /// <param name="operator">El operador que se utilizará para comparar los valores en la columna.</param>
    /// <remarks>
    /// Este método permite construir consultas SQL dinámicamente al aplicar filtros a las columnas especificadas.
    /// Se espera que la acción proporcionada modifique el objeto <see cref="ISqlBuilder"/> para agregar condiciones adicionales.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="Operator"/>
    void Where( string column, Action<ISqlBuilder> action, Operator @operator );
    /// <summary>
    /// Verifica si el valor de la columna especificada es nulo.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método se utiliza para comprobar si el valor de una columna en una base de datos o en una estructura de datos es nulo.
    /// Si el valor es nulo, se puede tomar una acción específica, como registrar un error o lanzar una excepción.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void IsNull( string column );
    /// <summary>
    /// Verifica que el valor de la columna especificada no sea nulo.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método lanza una excepción si el valor de la columna es nulo.
    /// Asegúrese de que el nombre de la columna proporcionado sea válido.
    /// </remarks>
    void IsNotNull(string column);
    /// <summary>
    /// Verifica si la columna especificada está vacía.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método comprueba si el contenido de la columna es nulo o una cadena vacía.
    /// Si la columna está vacía, se puede realizar una acción adicional según la lógica del programa.
    /// </remarks>
    /// <seealso cref="System.String.IsNullOrEmpty(string)"/>
    void IsEmpty(string column);
    /// <summary>
    /// Verifica si la columna especificada no está vacía.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método lanza una excepción si la columna está vacía o es nula.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza cuando el parámetro <paramref name="column"/> es nulo.</exception>
    /// <exception cref="ArgumentException">Se lanza cuando el parámetro <paramref name="column"/> está vacío.</exception>
    void IsNotEmpty(string column);
    /// <summary>
    /// Filtra los resultados de acuerdo a un rango definido por los parámetros mínimos y máximos.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará el filtro.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Define cómo se deben tratar los límites del rango.</param>
    /// <remarks>
    /// Este método permite establecer un filtro en una consulta, permitiendo especificar si los límites son inclusivos o exclusivos
    /// dependiendo del valor del parámetro <paramref name="boundary"/>.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    void Between( string column, int? min, int? max, Boundary boundary );
    /// <summary>
    /// Filtra los resultados basándose en un rango definido por valores mínimos y máximos.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará el filtro.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango.</param>
    /// <remarks>
    /// Este método permite establecer un filtro que puede incluir o excluir los límites 
    /// dependiendo de la configuración del parámetro <paramref name="boundary"/>.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    void Between( string column, double? min, double? max, Boundary boundary );
    /// <summary>
    /// Establece un rango para un valor decimal en una columna específica, considerando los límites definidos.
    /// </summary>
    /// <param name="column">El nombre de la columna en la que se aplicará el filtro.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">El tipo de límite que se aplicará (incluido/excluido).</param>
    /// <remarks>
    /// Este método permite filtrar registros en función de un rango de valores decimales, 
    /// proporcionando flexibilidad en la definición de los límites superior e inferior.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    void Between( string column, decimal? min, decimal? max, Boundary boundary );
    /// <summary>
    /// Filtra los registros de acuerdo a un rango de fechas especificado.
    /// </summary>
    /// <param name="column">El nombre de la columna que se utilizará para aplicar el filtro.</param>
    /// <param name="min">La fecha mínima del rango. Puede ser nula.</param>
    /// <param name="max">La fecha máxima del rango. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la hora en la comparación de fechas.</param>
    /// <param name="boundary">Especifica cómo se deben manejar los límites del rango. Puede ser nulo.</param>
    /// <remarks>
    /// Este método permite establecer un filtro en una consulta, limitando los resultados a aquellos que se encuentran
    /// dentro del rango de fechas definido por los parámetros <paramref name="min"/> y <paramref name="max"/>.
    /// Dependiendo del valor de <paramref name="includeTime"/>, la comparación puede considerar las horas, minutos y segundos.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    void Between( string column, DateTime? min, DateTime? max, bool includeTime, Boundary? boundary );
    /// <summary>
    /// Verifica si una condición existe en la base de datos utilizando el generador de SQL proporcionado.
    /// </summary>
    /// <param name="builder">El generador de SQL que se utilizará para construir la consulta.</param>
    /// <remarks>
    /// Este método ejecuta una consulta que determina si existen registros que cumplen con ciertos criterios.
    /// Asegúrese de que el <paramref name="builder"/> esté configurado correctamente antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Exists(ISqlBuilder builder);
    /// <summary>
    /// Verifica la existencia de una condición en la base de datos utilizando un constructor de consultas SQL.
    /// </summary>
    /// <param name="action">Acción que define la consulta SQL a ejecutar.</param>
    /// <remarks>
    /// Este método permite a los desarrolladores especificar una consulta SQL a través de una acción 
    /// que recibe un objeto que implementa <see cref="ISqlBuilder"/>. La consulta se ejecutará para 
    /// determinar si la condición especificada existe en la base de datos.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Exists( Action<ISqlBuilder> action );
    /// <summary>
    /// Realiza una operación que verifica la no existencia de un elemento en la base de datos.
    /// </summary>
    /// <param name="builder">El objeto <see cref="ISqlBuilder"/> que se utiliza para construir la consulta SQL.</param>
    /// <remarks>
    /// Este método se utiliza para construir una consulta que determina si un elemento específico no existe en la base de datos.
    /// Es útil en escenarios donde se necesita validar la ausencia de datos antes de realizar otras operaciones.
    /// </remarks>
    void NotExists(ISqlBuilder builder);
    /// <summary>
    /// Ejecuta una acción sobre un generador de SQL, asegurándose de que la entidad no exista.
    /// </summary>
    /// <param name="action">La acción que se ejecutará sobre el generador de SQL.</param>
    /// <remarks>
    /// Este método permite definir una lógica que se aplicará solo si la entidad especificada no existe en la base de datos.
    /// Es útil para operaciones de inserción o configuración que dependen de la ausencia de registros previos.
    /// </remarks>
    /// <example>
    /// <code>
    /// NotExists(builder => 
    /// {
    ///     // Lógica para manejar el caso donde la entidad no existe.
    /// });
    /// </code>
    /// </example>
    void NotExists( Action<ISqlBuilder> action );
    /// <summary>
    /// Agrega una consulta SQL a un conjunto existente de consultas.
    /// </summary>
    /// <param name="sql">La cadena de texto que representa la consulta SQL a agregar.</param>
    /// <param name="raw">Indica si la consulta SQL se debe tratar como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas SQL al agregar nuevas 
    /// instrucciones a un conjunto existente. Si el parámetro <paramref name="raw"/> 
    /// es verdadero, la consulta se añadirá tal como está, sin ningún tipo de 
    /// procesamiento adicional.
    /// </remarks>
    /// <example>
    /// <code>
    /// AppendSql("SELECT * FROM Usuarios", false);
    /// </code>
    /// </example>
    void AppendSql( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido o estado del objeto.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula WHERE actual, creando una nueva instancia de la misma.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>Una nueva instancia de <see cref="IWhereClause"/> que representa la cláusula WHERE clonada.</returns>
    /// <remarks>
    /// Este método es útil para crear copias de cláusulas WHERE que se pueden modificar sin afectar a la original.
    /// </remarks>
    IWhereClause Clone(SqlBuilderBase builder);
}
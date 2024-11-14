namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Representa una cláusula de unión en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para definir las operaciones y propiedades que deben implementarse
/// en las cláusulas de unión dentro de una consulta SQL.
/// </remarks>
public interface IJoinClause : ISqlClause {
    /// <summary>
    /// Une la tabla especificada a la consulta actual.
    /// </summary>
    /// <param name="table">El nombre de la tabla que se va a unir.</param>
    /// <remarks>
    /// Este método permite realizar una operación de unión en la consulta actual 
    /// utilizando la tabla proporcionada. Asegúrese de que la tabla exista en la base de datos 
    /// antes de llamar a este método para evitar errores.
    /// </remarks>
    /// <seealso cref="Split"/>
    void Join( string table );
    /// <summary>
    /// Une la consulta actual con otra tabla o conjunto de datos utilizando un alias especificado.
    /// </summary>
    /// <param name="builder">El objeto que construye la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará para la tabla unida en la consulta.</param>
    /// <remarks>
    /// Este método permite agregar una cláusula JOIN a la consulta SQL, facilitando la combinación de datos de múltiples tablas.
    /// Asegúrese de que el alias proporcionado sea único dentro del contexto de la consulta para evitar conflictos.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Join( ISqlBuilder builder, string alias );
    /// <summary>
    /// Une la consulta actual con otra utilizando el constructor proporcionado.
    /// </summary>
    /// <param name="action">Una acción que recibe un <see cref="ISqlBuilder"/> para construir la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará para la unión en la consulta.</param>
    /// <remarks>
    /// Este método permite agregar una unión a la consulta actual, facilitando la construcción de consultas más complejas.
    /// Asegúrese de que el alias proporcionado sea único dentro del contexto de la consulta.
    /// </remarks>
    void Join( Action<ISqlBuilder> action, string alias );
    /// <summary>
    /// Realiza una unión izquierda con la tabla especificada.
    /// </summary>
    /// <param name="table">El nombre de la tabla con la que se realizará la unión.</param>
    /// <remarks>
    /// La unión izquierda permite obtener todos los registros de la tabla principal y los registros coincidentes de la tabla secundaria.
    /// Si no hay coincidencias, se devolverán valores nulos para las columnas de la tabla secundaria.
    /// </remarks>
    void LeftJoin(string table);
    /// <summary>
    /// Realiza una unión izquierda en la consulta SQL utilizando el constructor de consultas proporcionado.
    /// </summary>
    /// <param name="builder">El constructor de consultas SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida en la consulta.</param>
    /// <remarks>
    /// Esta función se utiliza para combinar registros de dos tablas, donde se seleccionan todos los registros de la tabla de la izquierda 
    /// y los registros coincidentes de la tabla de la derecha. Si no hay coincidencias, se rellenan con valores nulos.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void LeftJoin( ISqlBuilder builder, string alias );
    /// <summary>
    /// Realiza una unión izquierda en la consulta SQL utilizando el generador de SQL especificado.
    /// </summary>
    /// <param name="action">Una acción que recibe un <see cref="ISqlBuilder"/> para construir la consulta SQL.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida en la consulta.</param>
    /// <remarks>
    /// La unión izquierda permite seleccionar todos los registros de la tabla principal y los registros coincidentes de la tabla unida.
    /// Si no hay coincidencias, se devolverán valores nulos para las columnas de la tabla unida.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void LeftJoin( Action<ISqlBuilder> action, string alias );
    /// <summary>
    /// Realiza una unión derecha en la tabla especificada.
    /// </summary>
    /// <param name="table">El nombre de la tabla sobre la cual se realizará la unión derecha.</param>
    /// <remarks>
    /// Esta función permite combinar datos de la tabla especificada con otra tabla, 
    /// asegurando que todos los registros de la tabla de la derecha se incluyan en el resultado, 
    /// incluso si no hay coincidencias en la tabla de la izquierda.
    /// </remarks>
    /// <seealso cref="LeftJoin"/>
    /// <seealso cref="InnerJoin"/>
    void RightJoin( string table );
    /// <summary>
    /// Realiza una unión derecha en la consulta SQL utilizando el constructor especificado.
    /// </summary>
    /// <param name="builder">El constructor SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla en la unión.</param>
    /// <remarks>
    /// La unión derecha asegura que todos los registros de la tabla de la derecha se incluyan en el resultado,
    /// incluso si no hay coincidencias en la tabla de la izquierda.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void RightJoin( ISqlBuilder builder, string alias );
    /// <summary>
    /// Realiza una unión a la derecha en la consulta SQL.
    /// </summary>
    /// <param name="action">Acción que se ejecutará sobre el generador de SQL.</param>
    /// <param name="alias">Alias que se utilizará para la tabla unida.</param>
    /// <remarks>
    /// Esta función permite especificar una acción personalizada que se aplicará al generador de SQL,
    /// permitiendo construir consultas complejas con uniones a la derecha.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void RightJoin( Action<ISqlBuilder> action, string alias );
    /// <summary>
    /// Ejecuta una operación en una columna específica con un valor dado.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se realizará la operación.</param>
    /// <param name="value">El valor que se utilizará en la operación.</param>
    /// <param name="@operator">El operador que se aplicará a la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la operación. Por defecto es <c>false</c>.</param>
    /// <remarks>
    /// Este método permite realizar comparaciones en bases de datos o estructuras de datos
    /// utilizando diferentes operadores, facilitando la construcción de consultas dinámicas.
    /// </remarks>
    /// <seealso cref="Operator"/>
    void On( string column, object value, Operator @operator = Operator.Equal, bool isParameterization = false );
    /// <summary>
    /// Agrega una cláusula JOIN a una consulta SQL existente.
    /// </summary>
    /// <param name="sql">La consulta SQL a la que se le añadirá la cláusula JOIN.</param>
    /// <param name="raw">Indica si la cláusula JOIN debe ser añadida en formato sin procesar.</param>
    /// <remarks>
    /// Este método permite modificar una consulta SQL existente para incluir una cláusula JOIN,
    /// lo que permite combinar datos de múltiples tablas. El parámetro <paramref name="raw"/> 
    /// determina si la cláusula se añade en un formato que no será procesado o manipulado 
    /// adicionalmente.
    /// </remarks>
    /// <example>
    /// <code>
    /// string consulta = "SELECT * FROM Tabla1";
    /// AppendJoin(consulta, true);
    /// </code>
    /// </example>
    void AppendJoin( string sql, bool raw );
    /// <summary>
    /// Agrega una cláusula de unión izquierda a la consulta SQL proporcionada.
    /// </summary>
    /// <param name="sql">La consulta SQL a la que se le añadirá la unión izquierda.</param>
    /// <param name="raw">Indica si la unión debe ser tratada como una consulta en bruto.</param>
    /// <remarks>
    /// Este método modifica la consulta SQL existente para incluir una cláusula LEFT JOIN,
    /// permitiendo combinar datos de dos o más tablas en función de una condición específica.
    /// </remarks>
    /// <example>
    /// <code>
    /// string consulta = "SELECT * FROM Tabla1";
    /// AppendLeftJoin(consulta, true);
    /// </code>
    /// </example>
    void AppendLeftJoin( string sql, bool raw );
    /// <summary>
    /// Agrega una cláusula de unión derecha a la consulta SQL proporcionada.
    /// </summary>
    /// <param name="sql">La consulta SQL a la que se le añadirá la unión derecha.</param>
    /// <param name="raw">Indica si la unión debe ser tratada como una consulta en bruto.</param>
    /// <remarks>
    /// Esta función modifica la consulta SQL original para incluir una cláusula 
    /// de unión derecha, permitiendo combinar registros de dos tablas donde 
    /// se incluyen todos los registros de la tabla de la derecha, incluso si 
    /// no hay coincidencias en la tabla de la izquierda.
    /// </remarks>
    /// <seealso cref="AppendLeftJoin"/>
    /// <seealso cref="AppendInnerJoin"/>
    void AppendRightJoin( string sql, bool raw );
    /// <summary>
    /// Agrega una instrucción SQL a un conjunto de comandos existentes.
    /// </summary>
    /// <param name="sql">La cadena de texto que representa la instrucción SQL a agregar.</param>
    /// <param name="raw">Un valor booleano que indica si la instrucción SQL debe ser tratada como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas SQL al agregar nuevas instrucciones 
    /// a un conjunto existente. Si el parámetro <paramref name="raw"/> se establece en <c>true</c>,
    /// la instrucción SQL se agregará sin ningún tipo de procesamiento adicional.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void AppendOn( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido o estado del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de unión actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta.</param>
    /// <returns>Una nueva instancia de <see cref="IJoinClause"/> que es una copia de la cláusula de unión actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de unión, lo que puede ser útil para modificar la cláusula sin afectar la original.
    /// </remarks>
    IJoinClause Clone(SqlBuilderBase builder);
}
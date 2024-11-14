namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Representa una cláusula de inserción en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y define los métodos y propiedades
/// necesarios para construir una cláusula de inserción en una sentencia SQL.
/// </remarks>
public interface IInsertClause : ISqlClause {
    /// <summary>
    /// Inserta datos en una tabla específica de la base de datos.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas en las que se insertarán los datos.</param>
    /// <param name="table">El nombre de la tabla en la que se insertarán los datos. Si es <c>null</c>, se utilizará la tabla predeterminada.</param>
    /// <remarks>
    /// Este método permite especificar las columnas en las que se desea insertar datos. 
    /// Si no se proporciona el nombre de la tabla, se utilizará una tabla predeterminada definida en la configuración.
    /// </remarks>
    /// <example>
    /// <code>
    /// Insert("Nombre, Edad", "Usuarios");
    /// </code>
    /// </example>
    void Insert( string columns, string table = null );
    /// <summary>
    /// Permite recibir un número variable de argumentos de tipo objeto.
    /// </summary>
    /// <param name="values">Una lista de valores de tipo objeto que se pasarán al método.</param>
    /// <remarks>
    /// Este método puede ser utilizado para procesar una colección de valores sin necesidad de definir un número fijo de parámetros.
    /// </remarks>
    void Values(params object[] values);
    /// <summary>
    /// Agrega una instrucción INSERT a la consulta SQL proporcionada.
    /// </summary>
    /// <param name="sql">La cadena SQL a la que se le añadirá la instrucción INSERT.</param>
    /// <param name="raw">Indica si la instrucción debe ser tratada como una consulta SQL en bruto.</param>
    /// <remarks>
    /// Este método modifica la consulta SQL existente para incluir una instrucción INSERT,
    /// permitiendo la inserción de datos en la base de datos. Si el parámetro <paramref name="raw"/>
    /// es verdadero, se asume que la cadena SQL proporcionada no requiere ningún tipo de
    /// procesamiento adicional.
    /// </remarks>
    void AppendInsert( string sql, bool raw );
    /// <summary>
    /// Agrega valores a una consulta SQL existente.
    /// </summary>
    /// <param name="sql">La cadena SQL a la que se agregarán los valores.</param>
    /// <param name="raw">Indica si los valores deben ser tratados como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite modificar una consulta SQL existente al agregarle nuevos valores.
    /// Si el parámetro <paramref name="raw"/> es verdadero, los valores se agregarán sin ningún tipo de procesamiento adicional,
    /// lo que puede ser útil en ciertos contextos donde se requiere insertar texto literal.
    /// </remarks>
    /// <seealso cref="System.Data.SqlClient.SqlCommand"/>
    void AppendValues( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido o estado del objeto.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados,
    /// eliminando cualquier dato o configuración que haya sido establecido previamente.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de inserción actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>Una nueva instancia de <see cref="IInsertClause"/> que es una copia de la cláusula actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de inserción, lo que puede ser útil para 
    /// modificar la cláusula sin afectar a la original.
    /// </remarks>
    IInsertClause Clone(SqlBuilderBase builder);
}
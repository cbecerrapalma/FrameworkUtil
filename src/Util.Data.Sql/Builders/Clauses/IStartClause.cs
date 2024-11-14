namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Define una cláusula de inicio para una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y se utiliza para representar
/// una parte específica de una consulta SQL que inicia una operación.
/// </remarks>
public interface IStartClause : ISqlClause {
    /// <summary>
    /// Establece una constante en el generador de consultas SQL.
    /// </summary>
    /// <param name="name">El nombre de la constante que se va a establecer.</param>
    /// <param name="builder">El generador de consultas SQL que se utilizará para establecer la constante.</param>
    void Cte( string name, ISqlBuilder builder );
    /// <summary>
    /// Agrega una consulta SQL a un conjunto de comandos existentes.
    /// </summary>
    /// <param name="sql">La consulta SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la consulta SQL debe ser tratada como cruda (true) o procesada (false).</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas SQL al agregar nuevas instrucciones 
    /// a las ya existentes. El parámetro <paramref name="raw"/> determina si la consulta se 
    /// debe ejecutar tal cual se proporciona o si debe ser procesada antes de su ejecución.
    /// </remarks>
    void Append(string sql, bool raw);
    /// <summary>
    /// Agrega una línea de texto a la consulta SQL.
    /// </summary>
    /// <param name="sql">La cadena de texto que representa la línea SQL a agregar.</param>
    /// <param name="raw">Indica si la línea se debe agregar como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas SQL al agregar líneas adicionales.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la línea se agrega tal cual, sin ningún tipo de procesamiento adicional.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void AppendLine( string sql, bool raw );
    /// <summary>
    /// Limpia los datos del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece todos los campos y propiedades del objeto a sus valores predeterminados.
    /// Se utiliza para preparar el objeto para un nuevo conjunto de datos o para liberar recursos.
    /// </remarks>
    void ClearCte();
    /// <summary>
    /// Limpia el contenido o estado del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados.
    /// Se debe llamar a este método cuando se desee reiniciar el objeto para su uso posterior.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de inicio actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para la clonación.</param>
    /// <returns>Una nueva instancia de <see cref="IStartClause"/> que es una copia de la cláusula de inicio actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de inicio, lo que puede ser útil para 
    /// modificar la cláusula sin afectar la original.
    /// </remarks>
    IStartClause Clone(SqlBuilderBase builder);
}
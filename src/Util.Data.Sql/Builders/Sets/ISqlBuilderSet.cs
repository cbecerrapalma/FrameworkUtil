namespace Util.Data.Sql.Builders.Sets; 

/// <summary>
/// Define una interfaz para construir consultas SQL.
/// </summary>
public interface ISqlBuilderSet {
    /// <summary>
    /// Combina múltiples instancias de <see cref="ISqlBuilder"/> en una única consulta SQL.
    /// </summary>
    /// <param name="builders">
    /// Un número variable de instancias de <see cref="ISqlBuilder"/> que se van a combinar.
    /// </param>
    /// <remarks>
    /// Este método permite agregar varias construcciones SQL en una sola operación,
    /// facilitando la creación de consultas más complejas mediante la unión de diferentes 
    /// componentes de construcción SQL.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Union( params ISqlBuilder[] builders );
    /// <summary>
    /// Combina múltiples instancias de <see cref="ISqlBuilder"/> en una sola consulta SQL.
    /// </summary>
    /// <param name="builders">Una colección de instancias que implementan <see cref="ISqlBuilder"/> que se unirán.</param>
    /// <remarks>
    /// Este método permite agregar múltiples constructores SQL en una única operación, 
    /// facilitando la creación de consultas más complejas a partir de componentes más simples.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Union( IEnumerable<ISqlBuilder> builders );
    /// <summary>
    /// Combina múltiples instancias de <see cref="ISqlBuilder"/> en una sola consulta SQL.
    /// </summary>
    /// <param name="builders">Una lista variable de instancias de <see cref="ISqlBuilder"/> que se combinarán.</param>
    /// <remarks>
    /// Este método permite agregar múltiples constructores de SQL en una única operación de unión.
    /// Es útil cuando se necesita combinar resultados de diferentes consultas en una sola.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void UnionAll( params ISqlBuilder[] builders );
    /// <summary>
    /// Combina múltiples instancias de <see cref="ISqlBuilder"/> en una sola consulta SQL.
    /// </summary>
    /// <param name="builders">Una colección de instancias de <see cref="ISqlBuilder"/> que se combinarán.</param>
    /// <remarks>
    /// Este método permite unir todas las consultas SQL generadas por los constructores proporcionados 
    /// en una única consulta. Es útil cuando se necesita consolidar múltiples consultas en una sola 
    /// operación para mejorar la eficiencia o simplificar la ejecución.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void UnionAll( IEnumerable<ISqlBuilder> builders );
    /// <summary>
    /// Realiza una intersección entre múltiples constructores SQL.
    /// </summary>
    /// <param name="builders">Una lista de constructores SQL que se utilizarán para crear la intersección.</param>
    /// <remarks>
    /// Este método permite combinar las consultas de los constructores SQL proporcionados
    /// utilizando la cláusula INTERSECT de SQL. Cada constructor debe estar configurado
    /// correctamente para generar una consulta válida.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Intersect( params ISqlBuilder[] builders );
    /// <summary>
    /// Realiza la intersección de múltiples constructores SQL.
    /// </summary>
    /// <param name="builders">Una colección de constructores SQL que se utilizarán para realizar la intersección.</param>
    /// <remarks>
    /// Este método toma una colección de objetos que implementan la interfaz <see cref="ISqlBuilder"/> 
    /// y combina sus resultados en una única consulta SQL que representa la intersección de todas las consultas proporcionadas.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Intersect( IEnumerable<ISqlBuilder> builders );
    /// <summary>
    /// Excluye las consultas especificadas en el conjunto de constructores SQL.
    /// </summary>
    /// <param name="builders">Una lista variable de constructores SQL que se deben excluir.</param>
    /// <remarks>
    /// Este método permite combinar múltiples constructores SQL y excluir aquellos que se 
    /// proporcionan como parámetros. Es útil para construir consultas complejas donde se 
    /// desea omitir ciertas partes de la consulta original.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Except( params ISqlBuilder[] builders );
    /// <summary>
    /// Realiza una operación de exclusión sobre una colección de constructores SQL.
    /// </summary>
    /// <param name="builders">Una colección de objetos que implementan la interfaz <see cref="ISqlBuilder"/> que se utilizarán para la operación de exclusión.</param>
    /// <remarks>
    /// Este método permite excluir los resultados generados por los constructores SQL especificados en la colección <paramref name="builders"/>.
    /// Asegúrese de que los constructores proporcionados sean válidos y estén correctamente configurados antes de llamar a este método.
    /// </remarks>
    void Except( IEnumerable<ISqlBuilder> builders );
    /// <summary>
    /// Limpia el contenido o estado del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados,
    /// permitiendo que el objeto sea reutilizado sin necesidad de crear una nueva instancia.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Convierte el objeto actual en un resultado representado como una cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el resultado del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para obtener una representación legible del objeto,
    /// que puede ser útil para la depuración o para mostrar información al usuario.
    /// </remarks>
    string ToResult();
    /// <summary>
    /// Clona una instancia de <see cref="SqlBuilderBase"/> y devuelve un nuevo objeto que implementa <see cref="ISqlBuilderSet"/>.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="SqlBuilderBase"/> que se desea clonar.</param>
    /// <returns>Una nueva instancia de <see cref="ISqlBuilderSet"/> que es una copia del <paramref name="builder"/> proporcionado.</returns>
    /// <remarks>
    /// Este método permite crear una copia independiente de un objeto <see cref="SqlBuilderBase"/> existente,
    /// lo que puede ser útil para modificar la copia sin afectar al original.
    /// </remarks>
    /// <seealso cref="ISqlBuilderSet"/>
    /// <seealso cref="SqlBuilderBase"/>
    ISqlBuilderSet Clone( SqlBuilderBase builder );
}
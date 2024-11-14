using Util.Data.Queries;

namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Representa una cláusula final en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y se utiliza para definir 
/// las características de una cláusula que se encuentra al final de una consulta SQL.
/// </remarks>
public interface IEndClause : ISqlClause {
    /// <summary>
    /// Omite una cantidad especificada de elementos.
    /// </summary>
    /// <param name="count">El número de elementos a omitir.</param>
    /// <remarks>
    /// Este método es útil cuando se desea saltar un número determinado de elementos en una colección o secuencia.
    /// Asegúrese de que el valor de <paramref name="count"/> no sea negativo.
    /// </remarks>
    void Skip(int count);
    /// <summary>
    /// Toma un número especificado de elementos.
    /// </summary>
    /// <param name="count">El número de elementos a tomar.</param>
    /// <remarks>
    /// Este método permite limitar la cantidad de elementos que se procesarán o se devolverán.
    /// Asegúrese de que el valor de <paramref name="count"/> sea mayor o igual a cero.
    /// </remarks>
    void Take(int count);
    /// <summary>
    /// Define un método que recibe una instancia de <see cref="IPage"/>.
    /// </summary>
    /// <param name="page">La instancia de <see cref="IPage"/> que se va a procesar.</param>
    /// <remarks>
    /// Este método puede ser utilizado para realizar operaciones específicas sobre la página
    /// proporcionada, como cargar contenido, aplicar estilos o manejar eventos.
    /// </remarks>
    void Page(IPage page);
    /// <summary>
    /// Agrega una instrucción SQL a un conjunto de consultas.
    /// </summary>
    /// <param name="sql">La cadena de texto que contiene la instrucción SQL a agregar.</param>
    /// <param name="raw">Indica si la instrucción SQL debe ser tratada como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite acumular múltiples instrucciones SQL para su posterior ejecución.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la instrucción SQL se agrega tal cual, 
    /// sin ningún tipo de procesamiento adicional.
    /// </remarks>
    void AppendSql(string sql, bool raw);
    /// <summary>
    /// Limpia el contenido de la página actual.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para restablecer todos los elementos visuales y datos de la página 
    /// a su estado inicial, eliminando cualquier información que el usuario haya ingresado o 
    /// que se haya mostrado anteriormente.
    /// </remarks>
    void ClearPage();
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos o datos presentes, 
    /// restableciendo el estado a su condición inicial.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de fin actual, creando una nueva instancia de la misma.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>Una nueva instancia de <see cref="IEndClause"/> que es una copia de la cláusula actual.</returns>
    /// <remarks>
    /// Este método es útil para crear copias de cláusulas que pueden ser modificadas sin afectar la cláusula original.
    /// </remarks>
    IEndClause Clone(SqlBuilderBase builder);
}
using Util.Data.Sql.Builders.Clauses;
using Util.Helpers;

namespace Util.Data.Sql.Builders.Sets;

/// <summary>
/// Representa un generador de consultas SQL para la cláusula SET.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlBuilderSet"/> y proporciona métodos para construir
/// la parte de la consulta SQL que se encarga de establecer valores en una instrucción UPDATE.
/// </remarks>
public class SqlBuilderSet : ISqlBuilderSet
{

    #region Campo

    protected ISqlBuilder MasterBuilder;
    protected List<SqlBuilderSetItem> SetItems;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlBuilderSet"/>.
    /// </summary>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir consultas SQL.</param>
    /// <param name="setItems">Una lista opcional de elementos de configuración de SQL, representados por <see cref="SqlBuilderSetItem"/>. Si se proporciona un valor nulo, se inicializa con una lista vacía.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    public SqlBuilderSet(ISqlBuilder builder, List<SqlBuilderSetItem> setItems = null)
    {
        MasterBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
        SetItems = setItems ?? new List<SqlBuilderSetItem>();
    }

    #endregion

    #region Set(Operaciones de conjunto)

    /// <summary>
    /// Establece los elementos de configuración en función del operador y los constructores SQL proporcionados.
    /// </summary>
    /// <param name="operator">El operador que se aplicará a cada elemento de configuración.</param>
    /// <param name="builders">Una colección de constructores SQL que se agregarán a los elementos de configuración.</param>
    /// <remarks>
    /// Si la colección de constructores SQL es nula, el método no realiza ninguna acción.
    /// Cada constructor SQL se envuelve en un objeto <see cref="SqlBuilderSetItem"/> junto con el operador especificado.
    /// </remarks>
    protected void Set(string @operator, IEnumerable<ISqlBuilder> builders)
    {
        if (builders == null)
            return;
        foreach (var builder in builders)
        {
            SetItems.Add(new SqlBuilderSetItem(@operator, builder));
        }
    }

    #endregion

    #region Union(Combinar conjuntos de resultados.)

    /// <summary>
    /// Realiza una operación de unión entre múltiples constructores SQL.
    /// </summary>
    /// <param name="builders">Una lista variable de constructores SQL que se unirán.</param>
    /// <remarks>
    /// Este método permite combinar los resultados de varias consultas SQL en una sola consulta.
    /// Se utiliza comúnmente en operaciones donde se desea obtener un conjunto de resultados que incluya
    /// registros de diferentes fuentes, siempre que las consultas tengan la misma cantidad de columnas
    /// y tipos compatibles.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public virtual void Union(params ISqlBuilder[] builders)
    {
        Set("Union", builders);
    }

    /// <summary>
    /// Realiza una operación de unión sobre una colección de constructores SQL.
    /// </summary>
    /// <param name="builders">Una colección de objetos que implementan la interfaz <see cref="ISqlBuilder"/> que se unirán.</param>
    public virtual void Union(IEnumerable<ISqlBuilder> builders)
    {
        Set("Union", builders);
    }

    #endregion

    #region UnionAll(Combinar conjuntos de resultados.)

    /// <summary>
    /// Combina el resultado de múltiples consultas SQL utilizando la cláusula "UNION ALL".
    /// </summary>
    /// <param name="builders">Una lista de instancias de <see cref="ISqlBuilder"/> que representan las consultas SQL a combinar.</param>
    /// <remarks>
    /// La cláusula "UNION ALL" permite incluir todos los registros de las consultas, incluyendo duplicados.
    /// Esta función es virtual, lo que permite que las clases derivadas puedan sobreescribirla si es necesario.
    /// </remarks>
    public virtual void UnionAll(params ISqlBuilder[] builders)
    {
        Set("Union All", builders);
    }

    /// <summary>
    /// Realiza una unión de todos los constructores SQL especificados utilizando la cláusula "UNION ALL".
    /// </summary>
    /// <param name="builders">Una colección de constructores SQL que se unirán.</param>
    public virtual void UnionAll(IEnumerable<ISqlBuilder> builders)
    {
        Set("Union All", builders);
    }

    #endregion

    #region Intersect(Intersección)

    /// <summary>
    /// Realiza una operación de intersección entre múltiples constructores SQL.
    /// </summary>
    /// <param name="builders">Una lista de constructores SQL que se utilizarán para la operación de intersección.</param>
    /// <remarks>
    /// Este método permite combinar los resultados de varias consultas SQL utilizando la cláusula INTERSECT.
    /// Cada constructor SQL proporcionado en el parámetro será utilizado para generar las consultas que se intersecarán.
    /// </remarks>
    public virtual void Intersect(params ISqlBuilder[] builders)
    {
        Set("Intersect", builders);
    }

    /// <summary>
    /// Realiza una intersección entre un conjunto de constructores SQL.
    /// </summary>
    /// <param name="builders">Una colección de constructores SQL que se utilizarán para realizar la intersección.</param>
    /// <remarks>
    /// Este método establece el tipo de operación como "Intersect" y aplica la colección de constructores SQL proporcionada.
    /// </remarks>
    public virtual void Intersect(IEnumerable<ISqlBuilder> builders)
    {
        Set("Intersect", builders);
    }

    #endregion

    #region Except(Diferencia de conjuntos.)

    /// <summary>
    /// Establece una cláusula EXCEPT en la consulta SQL.
    /// </summary>
    /// <param name="builders">Una lista de instancias de <see cref="ISqlBuilder"/> que representan las consultas a excluir.</param>
    public virtual void Except(params ISqlBuilder[] builders)
    {
        Set("Except", builders);
    }

    /// <summary>
    /// Realiza una operación de excepción sobre una colección de constructores SQL.
    /// </summary>
    /// <param name="builders">Una colección de objetos que implementan la interfaz <see cref="ISqlBuilder"/> que se utilizarán para la operación de excepción.</param>
    public virtual void Except(IEnumerable<ISqlBuilder> builders)
    {
        Set("Except", builders);
    }

    #endregion

    #region ToResult(Obtener resultados)

    /// <summary>
    /// Convierte el estado actual del objeto en un resultado en forma de cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el resultado. Si no hay elementos en el conjunto, se devuelve el resultado maestro; 
    /// de lo contrario, se devuelve el resultado del conjunto.
    /// </returns>
    public string ToResult()
    {
        if (SetItems.Count == 0)
            return GetMasterResult();
        return GetSetResult();
    }

    /// <summary>
    /// Obtiene el resultado maestro como una cadena de texto.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el resultado maestro generado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto StringBuilder para construir el resultado maestro
    /// al invocar el método AppendTo del objeto MasterBuilder.
    /// </remarks>
    protected virtual string GetMasterResult()
    {
        var builder = new StringBuilder();
        MasterBuilder.AppendTo(builder);
        return builder.ToString();
    }

    /// <summary>
    /// Genera una cadena que representa el resultado de un conjunto de operaciones SQL.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene el resultado de las operaciones SQL construidas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto StringBuilder para construir la cadena de resultado.
    /// Se invocan métodos auxiliares para agregar diferentes partes de la consulta SQL.
    /// </remarks>
    protected virtual string GetSetResult()
    {
        var builder = new StringBuilder();
        AppendMasterSql(builder);
        AppendSetSql(builder);
        AppendEndSql(builder);
        return builder.ToString();
    }

    /// <summary>
    /// Agrega la parte maestra de una consulta SQL al objeto StringBuilder proporcionado.
    /// </summary>
    /// <param name="result">El StringBuilder donde se construirá la consulta SQL.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para modificar el comportamiento
    /// de cómo se construye la consulta SQL. Se utiliza un accesor para obtener las diferentes partes de la consulta,
    /// que se añaden en el orden adecuado.
    /// </remarks>
    protected virtual void AppendMasterSql(StringBuilder result)
    {
        var accessor = ToSqlPartAccessor(MasterBuilder);
        AppendSql(result, accessor.StartClause);
        result.AppendLine("(");
        AppendSql(result, accessor.SelectClause);
        AppendSql(result, accessor.FromClause);
        AppendSql(result, accessor.JoinClause);
        AppendSql(result, accessor.WhereClause);
        AppendSql(result, accessor.GroupByClause);
        result.AppendLine(")");
    }

    /// <summary>
    /// Agrega una representación SQL de las cláusulas SET a un objeto StringBuilder.
    /// </summary>
    /// <param name="result">El objeto StringBuilder donde se añadirá la representación SQL.</param>
    /// <remarks>
    /// Este método itera sobre los elementos en <c>SetItems</c> y construye una cadena SQL 
    /// que representa las operaciones de actualización. Para cada elemento, se utiliza 
    /// un accesor para obtener las diferentes partes de la consulta SQL, como SELECT, 
    /// FROM, JOIN, WHERE y GROUP BY, que luego se añaden al resultado.
    /// </remarks>
    /// <seealso cref="SetItems"/>
    protected virtual void AppendSetSql(StringBuilder result)
    {
        foreach (var item in SetItems)
        {
            var accessor = ToSqlPartAccessor(item.Builder);
            result.AppendFormat("{0} ", item.Operator);
            result.AppendLine();
            result.AppendLine("(");
            AppendSql(result, accessor.SelectClause);
            AppendSql(result, accessor.FromClause);
            AppendSql(result, accessor.JoinClause);
            AppendSql(result, accessor.WhereClause);
            AppendSql(result, accessor.GroupByClause);
            result.AppendLine(")");
        }
    }

    /// <summary>
    /// Agrega la cláusula final de SQL al resultado especificado.
    /// </summary>
    /// <param name="result">El objeto <see cref="StringBuilder"/> donde se añadirá la cláusula SQL.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para modificar el comportamiento
    /// de cómo se agregan las cláusulas finales de SQL.
    /// </remarks>
    /// <seealso cref="ToSqlPartAccessor"/>
    /// <seealso cref="AppendSql"/>
    protected virtual void AppendEndSql(StringBuilder result)
    {
        var accessor = ToSqlPartAccessor(MasterBuilder);
        AppendSql(result, accessor.OrderByClause);
        AppendSql(result, accessor.EndClause);
        result.RemoveEnd($" {Common.Line}");
    }

    /// <summary>
    /// Convierte un objeto <see cref="ISqlBuilder"/> en un <see cref="ISqlPartAccessor"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="ISqlBuilder"/> que se desea convertir.</param>
    /// <returns>
    /// Un objeto <see cref="ISqlPartAccessor"/> que representa el <see cref="ISqlBuilder"/> proporcionado.
    /// </returns>
    /// <exception cref="NotImplementedException">Se lanza si el <paramref name="builder"/> no es de tipo <see cref="ISqlPartAccessor"/>.</exception>
    protected ISqlPartAccessor ToSqlPartAccessor(ISqlBuilder builder)
    {
        var accessor = builder as ISqlPartAccessor;
        if (accessor == null)
            throw new NotImplementedException(nameof(ISqlPartAccessor));
        return accessor;
    }

    /// <summary>
    /// Agrega una cláusula SQL al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le agregará la cláusula SQL.</param>
    /// <param name="content">La cláusula SQL que se va a agregar.</param>
    /// <remarks>
    /// Este método verifica primero si la cláusula SQL es válida mediante el método <see cref="ISqlClause.Validate"/>.
    /// Si la cláusula no es válida, no se realiza ninguna acción. Si es válida, se agrega al <paramref name="builder"/>
    /// y se añade un espacio en blanco al final.
    /// </remarks>
    protected void AppendSql(StringBuilder builder, ISqlClause content)
    {
        if (content.Validate() == false)
            return;
        content.AppendTo(builder);
        builder.AppendLine(" ");
    }

    #endregion

    #region Clear(Limpiar)

    /// <summary>
    /// Limpia todos los elementos de la colección de elementos.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección, dejando la colección vacía.
    /// </remarks>
    public void Clear()
    {
        SetItems.Clear();
    }

    #endregion

    #region Clone(Copiar conjunto de generadores de SQL)

    /// <inheritdoc />
    /// <summary>
    /// Clona el conjunto de constructores SQL basado en el constructor proporcionado.
    /// </summary>
    /// <param name="builder">El constructor SQL base que se utilizará para crear la nueva instancia.</param>
    /// <returns>Una nueva instancia de <see cref="ISqlBuilderSet"/> que es una copia del conjunto actual.</returns>
    /// <remarks>
    /// Este método crea una copia profunda de los elementos del conjunto, asegurando que los cambios en la copia no afecten al original.
    /// </remarks>
    /// <seealso cref="ISqlBuilderSet"/>
    /// <seealso cref="SqlBuilderBase"/>
    public virtual ISqlBuilderSet Clone(SqlBuilderBase builder)
    {
        return new SqlBuilderSet(builder, SetItems.Select(t => t.Clone()).ToList());
    }

    #endregion
}
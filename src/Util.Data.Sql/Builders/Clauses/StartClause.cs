using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Core;
using Util.Helpers;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de inicio en una estructura de cláusulas.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ClauseBase"/> y 
/// implementa la interfaz <see cref="IStartClause"/>. 
/// Se utiliza para definir el comportamiento y las propiedades 
/// específicas de una cláusula de inicio en el contexto de 
/// una consulta o expresión.
/// </remarks>
public class StartClause : ClauseBase, IStartClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected List<CteItem> CteItems;
    protected readonly IColumnCache ColumnCache;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StartClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">
    /// El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.
    /// </param>
    /// <param name="result">
    /// Un <see cref="StringBuilder"/> opcional que contendrá el resultado de la construcción de la consulta. 
    /// Si se proporciona como <c>null</c>, se inicializará uno nuevo.
    /// </param>
    /// <param name="cteItems">
    /// Una lista opcional de elementos CTE (<see cref="CteItem"/>) que se utilizarán en la cláusula. 
    /// Si se proporciona como <c>null</c>, se inicializará una nueva lista.
    /// </param>
    public StartClause(SqlBuilderBase sqlBuilder, StringBuilder result = null, List<CteItem> cteItems = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        CteItems = cteItems ?? new List<CteItem>();
        ColumnCache = sqlBuilder.ColumnCache;
    }

    #endregion

    #region Cte(Expresión de tabla común CTE)

    /// <summary>
    /// Agrega un nuevo elemento CTE (Common Table Expression) a la colección de elementos CTE.
    /// </summary>
    /// <param name="name">El nombre del CTE que se va a agregar. No debe estar vacío.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que define la consulta SQL asociada al CTE.</param>
    /// <remarks>
    /// Si el nombre proporcionado está vacío o si el constructor <paramref name="builder"/> es nulo, 
    /// el método no realizará ninguna acción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    public void Cte(string name, ISqlBuilder builder)
    {
        if (name.IsEmpty() || builder == null)
            return;
        name = ColumnCache.GetSafeColumn(name);
        CteItems.Add(new CteItem(name, builder));
    }

    #endregion

    #region Append(Agregar al inicio.)

    /// <summary>
    /// Agrega una cadena SQL al resultado, procesándola según el valor del parámetro <paramref name="raw"/>.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar al resultado.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada sin procesar (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si la cadena <paramref name="sql"/> es nula o está vacía, no se realizará ninguna acción.
    /// Si <paramref name="raw"/> es verdadero, se agrega la cadena SQL tal como está.
    /// Si <paramref name="raw"/> es falso, se aplica un procesamiento a la cadena SQL antes de agregarla.
    /// </remarks>
    public void Append(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            Result.Append(sql);
            return;
        }
        Result.Append(ReplaceRawSql(sql));
    }

    #endregion

    #region AppendLine(Agregar al inicio y saltar línea.)

    /// <summary>
    /// Agrega una línea de texto SQL al resultado.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada tal cual (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si la cadena SQL está vacía o es solo espacios en blanco, no se realizará ninguna acción.
    /// Si el parámetro <paramref name="raw"/> es verdadero, se agrega la cadena SQL sin modificaciones.
    /// Si el parámetro <paramref name="raw"/> es falso, se procesará la cadena SQL mediante el método <see cref="ReplaceRawSql(string)"/> antes de agregarla.
    /// </remarks>
    public void AppendLine(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            Result.AppendLine(sql);
            return;
        }
        Result.AppendLine(ReplaceRawSql(sql));
    }

    #endregion

    #region Validate(Verificación)

    /// <summary>
    /// Valida si hay resultados o elementos en la lista de CteItems.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si hay resultados o si la lista de CteItems contiene elementos; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public bool Validate()
    {
        if (Result.Length > 0)
            return true;
        if (CteItems.Count > 0)
            return true;
        return false;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <summary>
    /// Agrega el resultado a un objeto <see cref="StringBuilder"/> dado.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregará el resultado.</param>
    /// <remarks>
    /// Este método verifica primero si el objeto <paramref name="builder"/> es nulo.
    /// Si la validación falla, el método no realiza ninguna acción.
    /// En caso contrario, se llama al método <see cref="AppendCte"/> y se agrega el resultado.
    /// </remarks>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        AppendCte(builder);
        builder.Append(Result);
    }

    #endregion

    #region AppendCte(Agregar expresión de tabla común CTE)

    /// <summary>
    /// Agrega elementos de CTE (Common Table Expressions) al objeto StringBuilder proporcionado.
    /// </summary>
    /// <param name="builder">El StringBuilder al que se agregarán los elementos de CTE.</param>
    /// <remarks>
    /// Este método verifica si hay elementos en la colección CteItems. Si no hay elementos, no realiza ninguna acción.
    /// Si hay elementos, se agrega la palabra clave correspondiente de CTE y se itera sobre cada elemento,
    /// formateando su nombre y su representación en el StringBuilder.
    /// </remarks>
    protected virtual void AppendCte(StringBuilder builder)
    {
        if (CteItems.Count == 0)
            return;
        builder.AppendFormat("{0} ", GetCteKeyWord());
        foreach (var item in CteItems)
        {
            builder.Append(item.Name);
            builder.AppendLine(" ");
            builder.Append("As (");
            item.Builder.AppendTo(builder);
            builder.AppendLine("),");
        }
        builder.RemoveEnd($",{Common.Line}");
    }

    /// <summary>
    /// Obtiene la palabra clave utilizada para la cláusula "With".
    /// </summary>
    /// <returns>
    /// Una cadena que representa la palabra clave "With".
    /// </returns>
    protected virtual string GetCteKeyWord()
    {
        return "With";
    }

    #endregion

    #region ClearCte(Limpiar la expresión de tabla común (CTE))

    /// <summary>
    /// Limpia todos los elementos de la colección CteItems.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la lista o colección CteItems,
    /// dejándola vacía. Es útil para reiniciar el estado de la colección sin
    /// necesidad de crear una nueva instancia.
    /// </remarks>
    public void ClearCte()
    {
        CteItems.Clear();
    }

    #endregion

    #region Clear(Limpiar)

    /// <summary>
    /// Limpia los resultados y los elementos de CteItems.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de las colecciones Result y CteItems,
    /// dejándolas vacías. Es útil para reiniciar el estado de la clase sin necesidad
    /// de crear una nueva instancia.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
        CteItems.Clear();
    }

    #endregion

    #region Clone(Copiar cláusula de inicio)

    /// <summary>
    /// Clona la cláusula de inicio actual, creando una nueva instancia de <see cref="StartClause"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IStartClause"/> que representa la cláusula de inicio clonada.
    /// </returns>
    /// <remarks>
    /// Este método crea una copia de la cláusula de inicio actual, incluyendo el resultado y los elementos CTE clonados.
    /// </remarks>
    public virtual IStartClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new StartClause(builder, result, CteItems.Select(t => t.Clone()).ToList());
    }

    #endregion
}
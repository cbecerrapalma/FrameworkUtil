using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de ordenamiento en una consulta.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para definir el orden en que se deben presentar los resultados de una consulta.
/// </remarks>
public class OrderByClause : ClauseBase, IOrderByClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected readonly IColumnCache ColumnCache;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrderByClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> opcional que contendrá el resultado de la cláusula. Si es <c>null</c>, se inicializa uno nuevo.</param>
    /// <remarks>
    /// Esta clase se encarga de construir la cláusula ORDER BY de una consulta SQL.
    /// </remarks>
    public OrderByClause(SqlBuilderBase sqlBuilder, StringBuilder result = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
    }

    #endregion

    #region OrderBy(Ordenar)

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos según el criterio especificado en la cadena de orden.
    /// </summary>
    /// <param name="order">Una cadena que contiene los criterios de ordenación, separados por comas.</param>
    /// <remarks>
    /// Este método divide la cadena de orden en elementos individuales y los procesa uno por uno.
    /// Si la cadena de orden está vacía o contiene solo espacios en blanco, el método no realiza ninguna acción.
    /// Cada elemento se convierte en un objeto <see cref="OrderByItem"/> para determinar la columna y si debe ordenarse de forma descendente.
    /// Los resultados se añaden a un objeto de tipo <see cref="StringBuilder"/> llamado <c>Result</c>.
    /// </remarks>
    /// <seealso cref="OrderByItem"/>
    /// <seealso cref="ColumnCache.GetSafeColumn(string)"/>
    public void OrderBy(string order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return;
        var items = order.Split(',').Where(t => t.IsEmpty() == false);
        foreach (var item in items)
        {
            var orderItem = new OrderByItem(item);
            Result.Append(ColumnCache.GetSafeColumn(orderItem.Column));
            if (orderItem.Desc)
                Result.Append(" Desc");
            Result.Append(",");
        }
    }

    #endregion

    #region AppendSql(Agregar a la cláusula de ordenamiento.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cadena SQL al resultado, procesándola según el valor del parámetro <paramref name="raw"/>.
    /// </summary>
    /// <param name="sql">La cadena SQL que se va a agregar al resultado. Si es nula o está vacía, no se realiza ninguna acción.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada tal cual (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si <paramref name="raw"/> es verdadero, la cadena SQL se agrega directamente al resultado. 
    /// Si es falso, se aplica un procesamiento a la cadena SQL antes de agregarla.
    /// </remarks>
    public void AppendSql(string sql, bool raw)
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

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado tiene una longitud mayor a cero.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el resultado es válido (longitud mayor a cero); de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay algún resultado disponible.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega la representación de la cláusula "Order By" al objeto <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se añadirá la cláusula.</param>
    /// <remarks>
    /// Este método verifica primero si el objeto <paramref name="builder"/> es nulo. 
    /// Si la validación falla, no se realiza ninguna acción. 
    /// Si la validación es exitosa, se añade la cadena "Order By " seguida del resultado, 
    /// y se elimina la última coma si está presente.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="StringBuilder"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("Order By ");
        builder.Append(Result);
        builder.RemoveEnd(",");
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia el contenido del resultado.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de resultados.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
    }

    #endregion

    #region Clone(Copiar cláusula Order By.)

    /// <inheritdoc />
    /// <summary>
    /// Clona la cláusula de ordenamiento actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IOrderByClause"/> que representa la cláusula de ordenamiento clonada.
    /// </returns>
    /// <remarks>
    /// Este método crea una copia de la cláusula de ordenamiento actual, utilizando el 
    /// <paramref name="builder"/> proporcionado para inicializar la nueva instancia.
    /// </remarks>
    public virtual IOrderByClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new OrderByClause(builder, result);
    }

    #endregion
}
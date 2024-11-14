using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Core;
using Util.Data.Sql.Builders.Params;
using Util.Helpers;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de inserción en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase permite construir y manipular una cláusula de inserción, 
/// facilitando la creación de sentencias SQL para insertar datos en una base de datos.
/// </remarks>
public class InsertClause : ClauseBase, IInsertClause
{

    #region Campo

    protected readonly StringBuilder InsertResult;
    protected readonly StringBuilder ValuesResult;
    protected readonly IColumnCache ColumnCache;
    protected readonly IParameterManager ParameterManager;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InsertClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="insertResult">Una instancia opcional de <see cref="StringBuilder"/> que contendrá el resultado de la cláusula INSERT. Si es null, se inicializa una nueva instancia.</param>
    /// <param name="valuesResult">Una instancia opcional de <see cref="StringBuilder"/> que contendrá el resultado de los valores a insertar. Si es null, se inicializa una nueva instancia.</param>
    /// <remarks>
    /// Esta clase se utiliza para construir la cláusula INSERT de una consulta SQL, gestionando los resultados de la inserción y los valores correspondientes.
    /// </remarks>
    public InsertClause(SqlBuilderBase sqlBuilder, StringBuilder insertResult = null, StringBuilder valuesResult = null) : base(sqlBuilder)
    {
        InsertResult = insertResult ?? new StringBuilder();
        ValuesResult = valuesResult ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
        ParameterManager = sqlBuilder.ParameterManager;
    }

    #endregion

    #region Insert(Configurar el conjunto de nombres de tablas y columnas a insertar.)

    /// <inheritdoc />
    /// <summary>
    /// Inserta columnas en la sentencia de inserción para una tabla específica.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a insertar.</param>
    /// <param name="table">Opcional. El nombre de la tabla en la que se insertarán las columnas. Si no se proporciona, se utilizará el valor predeterminado.</param>
    /// <remarks>
    /// Este método verifica si ya se ha inicializado una sentencia de inserción. Si no se ha inicializado y el nombre de la tabla está vacío, el método no realiza ninguna acción.
    /// Si la sentencia de inserción ya contiene resultados, se añaden las columnas especificadas a la sentencia existente.
    /// </remarks>
    /// <seealso cref="ColumnCache"/>
    /// <seealso cref="TableItem"/>
    public void Insert(string columns, string table = null)
    {
        if (InsertResult.Length == 0)
        {
            if (table.IsEmpty())
                return;
            var tableItem = new TableItem(Dialect, table);
            tableItem.AppendTo(InsertResult);
            InsertResult.Append("(");
            InsertResult.Append(ColumnCache.GetSafeColumns(columns));
            InsertResult.Append(")");
            return;
        }
        AppendColumns(ColumnCache.GetSafeColumns(columns));
    }

    /// <summary>
    /// Agrega columnas a la cadena de resultado de inserción.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a agregar.</param>
    /// <remarks>
    /// Este método elimina el carácter de cierre ')' al final de la cadena de resultado,
    /// luego agrega una coma y las columnas proporcionadas, y finalmente vuelve a agregar
    /// el carácter de cierre ')' al final de la cadena.
    /// </remarks>
    protected void AppendColumns(string columns)
    {
        InsertResult.RemoveEnd(")");
        InsertResult.Append(",");
        InsertResult.Append(columns);
        InsertResult.Append(")");
    }

    #endregion

    #region Values(Configurar el conjunto de valores a insertar.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega un conjunto de valores a la colección de resultados.
    /// </summary>
    /// <param name="values">Un arreglo de objetos que representan los valores a agregar.</param>
    /// <remarks>
    /// Este método verifica si el arreglo de valores es nulo. Si no lo es, 
    /// se agrega una coma antes de los nuevos valores si ya hay valores existentes.
    /// Luego, se generan nombres de parámetros únicos para cada valor y se 
    /// almacenan en el administrador de parámetros.
    /// </remarks>
    /// <seealso cref="ParameterManager"/>
    public void Values(params object[] values)
    {
        if (values == null)
            return;
        if (ValuesResult.Length > 0)
            ValuesResult.Append(",");
        ValuesResult.Append("(");
        foreach (var value in values)
        {
            var paramName = ParameterManager.GenerateName();
            ValuesResult.AppendFormat("{0},", paramName);
            ParameterManager.Add(paramName, value);
        }
        ValuesResult.RemoveEnd(",").Append(")");
    }

    #endregion

    #region AppendInsert(Agregar a la cláusula Insertar)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una instrucción SQL de inserción al resultado.
    /// </summary>
    /// <param name="sql">La cadena de texto que representa la instrucción SQL a agregar.</param>
    /// <param name="raw">Indica si la instrucción SQL es en formato bruto. Si es verdadero, se agrega tal cual; de lo contrario, se procesa antes de agregar.</param>
    /// <remarks>
    /// Si la cadena <paramref name="sql"/> es nula o está vacía, no se realizará ninguna acción.
    /// </remarks>
    public void AppendInsert(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            InsertResult.Append(sql);
            return;
        }
        InsertResult.Append(ReplaceRawSql(sql));
    }

    #endregion

    #region AppendValues(Agregar a la cláusula Values)

    /// <inheritdoc />
    /// <summary>
    /// Agrega valores a la colección de resultados.
    /// </summary>
    /// <param name="sql">La cadena SQL que se va a agregar. No debe estar vacía ni ser nula.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada sin modificaciones.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="sql"/> está vacío o es nulo, el método no realizará ninguna acción.
    /// Si <paramref name="raw"/> es verdadero, se agrega la cadena SQL tal cual. 
    /// Si es falso, se aplica un procesamiento a la cadena SQL antes de agregarla.
    /// </remarks>
    public void AppendValues(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            ValuesResult.Append(sql);
            return;
        }
        ValuesResult.Append(ReplaceRawSql(sql));
    }

    #endregion

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado de inserción tiene longitud diferente de cero.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el resultado de inserción contiene elementos; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si se ha realizado alguna inserción exitosa.
    /// </remarks>
    public bool Validate()
    {
        return InsertResult.Length != 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una representación en forma de cadena de la operación de inserción 
    /// al objeto <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se 
    /// agregará la representación de la operación de inserción.</param>
    /// <remarks>
    /// Este método verifica primero si el objeto <paramref name="builder"/> es nulo 
    /// y si la validación de la operación de inserción es exitosa antes de 
    /// proceder a agregar la información. Si no hay valores para insertar, 
    /// se eliminará la última línea agregada.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="InsertResult"/>
    /// <seealso cref="ValuesResult"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("Insert Into ");
        builder.Append(InsertResult);
        builder.AppendLine(" ");
        if (ValuesResult.Length == 0)
        {
            builder.RemoveEnd(Common.Line);
            return;
        }
        builder.Append("Values");
        builder.Append(ValuesResult);
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia los resultados de inserción y los valores almacenados.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de las colecciones <c>InsertResult</c> y <c>ValuesResult</c>.
    /// </remarks>
    public void Clear()
    {
        InsertResult.Clear();
        ValuesResult.Clear();
    }

    #endregion

    #region Clone(Copiar la cláusula Insertar)

    /// <inheritdoc />
    /// <summary>
    /// Clona la cláusula de inserción actual, creando una nueva instancia de <see cref="InsertClause"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula de inserción.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IInsertClause"/> que representa la cláusula de inserción clonada.
    /// </returns>
    /// <remarks>
    /// Este método crea una copia de los resultados de inserción y valores actuales, 
    /// permitiendo que se realicen modificaciones en la nueva instancia sin afectar a la original.
    /// </remarks>
    /// <seealso cref="InsertClause"/>
    public virtual IInsertClause Clone(SqlBuilderBase builder)
    {
        var insertResult = new StringBuilder();
        insertResult.Append(InsertResult);
        var valuesResult = new StringBuilder();
        valuesResult.Append(ValuesResult);
        return new InsertClause(builder, insertResult, valuesResult);
    }

    #endregion
}
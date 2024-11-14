using Util.Data.Queries;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula final en una estructura de cláusulas.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ClauseBase"/> y implementa la interfaz <see cref="IEndClause"/>.
/// Se utiliza para definir el comportamiento y las propiedades de una cláusula que marca el final de una serie de condiciones o instrucciones.
/// </remarks>
public class EndClause : ClauseBase, IEndClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected string OffsetParam;
    protected string LimitParam;
    protected IParameterManager ParameterManager;
    protected IPage Pager;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EndClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> que contendrá el resultado de la consulta. Si es null, se inicializa uno nuevo.</param>
    /// <param name="offsetParam">El parámetro de desplazamiento (offset) que se utilizará en la consulta.</param>
    /// <param name="limitParam">El parámetro de límite (limit) que se utilizará en la consulta.</param>
    /// <param name="pager">Una instancia de <see cref="IPage"/> que se utilizará para la paginación de resultados.</param>
    /// <remarks>
    /// Este constructor permite establecer los parámetros necesarios para la cláusula final de una consulta SQL,
    /// incluyendo la posibilidad de paginación y el manejo de parámetros.
    /// </remarks>
    public EndClause(SqlBuilderBase sqlBuilder, StringBuilder result = null, string offsetParam = null, string limitParam = null, IPage pager = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        OffsetParam = offsetParam;
        LimitParam = limitParam;
        Pager = pager;
        ParameterManager = sqlBuilder.ParameterManager;
    }

    #endregion

    #region Page(Configurar la paginación.)

    /// <inheritdoc />
    /// <summary>
    /// Establece la página actual y configura los parámetros de paginación.
    /// </summary>
    /// <param name="page">La instancia de <see cref="IPage"/> que contiene la información de la página.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="page"/> es nulo, el método no realiza ninguna acción.
    /// Este método actualiza el objeto <c>Pager</c> y ajusta la cantidad de elementos a omitir y a tomar
    /// según los valores proporcionados por la instancia de <see cref="IPage"/>.
    /// </remarks>
    public void Page(IPage page)
    {
        if (page == null)
            return;
        Pager = page;
        Skip(page.GetSkipCount());
        Take(page.PageSize);
    }

    #endregion

    #region Skip(Configurar el número de filas a omitir.)

    /// <inheritdoc />
    /// <summary>
    /// Salta un número especificado de elementos en una secuencia.
    /// </summary>
    /// <param name="count">El número de elementos a saltar.</param>
    /// <remarks>
    /// Este método utiliza un parámetro de desplazamiento obtenido mediante el método <see cref="GetOffsetParam"/> 
    /// y lo añade a la gestión de parámetros a través de <see cref="ParameterManager.Add"/>.
    /// </remarks>
    /// <seealso cref="GetOffsetParam"/>
    /// <seealso cref="ParameterManager"/>
    public void Skip(int count)
    {
        var param = GetOffsetParam();
        ParameterManager.Add(param, count);
    }

    /// <summary>
    /// Obtiene el parámetro de desplazamiento (offset).
    /// </summary>
    /// <returns>
    /// Devuelve el parámetro de desplazamiento como una cadena.
    /// Si el parámetro de desplazamiento ya está definido, se devuelve el valor existente.
    /// De lo contrario, se crea un nuevo parámetro de desplazamiento.
    /// </returns>
    protected string GetOffsetParam()
    {
        if (OffsetParam.IsEmpty() == false)
            return OffsetParam;
        OffsetParam = CreateOffsetParam();
        return OffsetParam;
    }

    /// <summary>
    /// Crea un parámetro de desplazamiento y lo agrega al administrador de parámetros.
    /// </summary>
    /// <returns>
    /// Devuelve el nombre del parámetro de desplazamiento creado.
    /// </returns>
    private string CreateOffsetParam()
    {
        var result = ParameterManager.GenerateName();
        ParameterManager.Add(result, 0);
        return result;
    }

    #endregion

    #region Take(Establecer el número de filas para obtener)

    /// <inheritdoc />
    /// <summary>
    /// Toma una cantidad específica de elementos y los añade a la gestión de parámetros.
    /// </summary>
    /// <param name="count">El número de elementos a tomar.</param>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetLimitParam"/> para obtener el parámetro límite 
    /// y luego añade la cantidad especificada a la gestión de parámetros mediante 
    /// <see cref="ParameterManager.Add(int, int)"/>.
    /// </remarks>
    public void Take(int count)
    {
        var param = GetLimitParam();
        ParameterManager.Add(param, count);
    }

    /// <summary>
    /// Obtiene el parámetro de límite. Si el parámetro de límite ya está definido, lo devuelve.
    /// De lo contrario, genera un nuevo nombre de parámetro y lo asigna.
    /// </summary>
    /// <returns>
    /// Un string que representa el parámetro de límite. 
    /// Si ya existe, se devuelve el existente; de lo contrario, se devuelve un nuevo nombre generado.
    /// </returns>
    protected string GetLimitParam()
    {
        if (LimitParam.IsEmpty() == false)
            return LimitParam;
        LimitParam = ParameterManager.GenerateName();
        return LimitParam;
    }

    #endregion

    #region AppendSql(Agregar a la posición final.)

    /// <summary>
    /// Agrega una cadena SQL al resultado, procesándola según el parámetro especificado.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar al resultado.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada sin procesar (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si la cadena SQL está vacía o es solo espacios en blanco, no se realizará ninguna acción.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la cadena SQL se agrega tal como está.
    /// Si el parámetro <paramref name="raw"/> es falso, la cadena SQL se procesa mediante el método <see cref="ReplaceRawSql(string)"/> antes de ser agregada.
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

    #region Validate(Verificar)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado es válido basado en ciertas condiciones.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el resultado tiene longitud mayor a cero o si el parámetro límite no está vacío; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para determinar si los criterios de validación se cumplen.
    /// </remarks>
    public bool Validate()
    {
        if (Result.Length > 0)
            return true;
        if (LimitParam.IsEmpty() == false)
            return true;
        return false;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega el resultado a un objeto <see cref="StringBuilder"/> dado,
    /// siempre y cuando la validación sea exitosa.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregará el resultado.</param>
    /// <remarks>
    /// Este método primero verifica si el objeto <paramref name="builder"/> es nulo,
    /// y si la validación falla, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="AppendLimit"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        AppendLimit(builder);
        builder.Append(Result);
    }

    #endregion

    #region AppendLimit(Agregar límite de filas)

    /// <summary>
    /// Agrega una cláusula de límite a la consulta SQL representada por el objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la cláusula de límite.</param>
    /// <remarks>
    /// Este método verifica si el parámetro de límite está vacío antes de intentar agregar la cláusula.
    /// Si el parámetro de límite no está vacío, se formatea y se agrega una cadena que incluye
    /// el límite y el desplazamiento a la consulta.
    /// </remarks>
    /// <seealso cref="GetLimitParam"/>
    /// <seealso cref="GetOffsetParam"/>
    protected virtual void AppendLimit(StringBuilder builder)
    {
        if (LimitParam.IsEmpty())
            return;
        builder.AppendFormat("Limit {0} OFFSET {1}", GetLimitParam(), GetOffsetParam());
    }

    #endregion

    #region ClearPage(Limpiar paginación)

    /// <inheritdoc />
    /// <summary>
    /// Limpia los parámetros de paginación establecidos.
    /// </summary>
    /// <remarks>
    /// Este método establece los parámetros de desplazamiento (OffsetParam), 
    /// límite (LimitParam) y paginador (Pager) a null, 
    /// lo que significa que se reinicia el estado de la paginación.
    /// </remarks>
    public void ClearPage()
    {
        OffsetParam = null;
        LimitParam = null;
        Pager = null;
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia los resultados y restablece la página actual.
    /// </summary>
    /// <remarks>
    /// Este método llama al método <see cref="ClearPage"/> para realizar una limpieza adicional
    /// después de vaciar los resultados.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
        ClearPage();
    }

    #endregion

    #region Clone(copiar cláusula final)

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia de la cláusula final actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IEndClause"/> que representa la copia de la cláusula final.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StringBuilder"/> para construir el resultado de la cláusula final
    /// y copia los parámetros de desplazamiento y límite de la instancia actual.
    /// </remarks>
    public virtual IEndClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new EndClause(builder, result, OffsetParam, LimitParam, Pager);
    }

    #endregion
}
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders; 

/// <summary>
/// Representa el resultado de una construcción de consulta SQL.
/// </summary>
public class SqlBuilderResult {
    private readonly string _sql;
    private readonly List<SqlParam> _sqlParams;
    private readonly IParamLiteralsResolver _resolver;
    private readonly IParameterManager _parameterManager;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlBuilderResult"/>.
    /// </summary>
    /// <param name="sql">La cadena SQL que se va a construir.</param>
    /// <param name="sqlParams">Una lista de parámetros SQL que se utilizarán en la consulta.</param>
    /// <param name="resolver">Una instancia de <see cref="IParamLiteralsResolver"/> que se encargará de resolver los literales de los parámetros.</param>
    /// <param name="parameterManager">Una instancia de <see cref="IParameterManager"/> que gestionará los parámetros de la consulta.</param>
    /// <remarks>
    /// Si <paramref name="sqlParams"/> es nulo, se inicializará como una lista vacía.
    /// </remarks>
    public SqlBuilderResult( string sql, List<SqlParam> sqlParams, IParamLiteralsResolver resolver, IParameterManager parameterManager ) {
        _sql = sql;
        _sqlParams = sqlParams ?? new List<SqlParam>();
        _resolver = resolver;
        _parameterManager = parameterManager;
    }

    /// <summary>
    /// Obtiene la cadena SQL almacenada.
    /// </summary>
    /// <returns>
    /// La cadena SQL que se ha almacenado.
    /// </returns>
    public string GetSql() {
        return _sql;
    }

    /// <summary>
    /// Obtiene la lista de parámetros SQL.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="SqlParam"/> que representan los parámetros SQL.
    /// </returns>
    public List<SqlParam> GetParams() {
        return _sqlParams;
    }

    /// <summary>
    /// Obtiene el valor de un parámetro de tipo T a partir de su nombre.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="name">El nombre del parámetro que se desea recuperar.</param>
    /// <returns>El valor del parámetro convertido al tipo T, o el valor predeterminado de T si el parámetro no se encuentra.</returns>
    /// <remarks>
    /// Este método normaliza el nombre del parámetro antes de buscarlo en la colección de parámetros SQL.
    /// Si el parámetro no se encuentra, se devuelve el valor predeterminado del tipo especificado.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert"/>
    public T GetParam<T>( string name ) {
        name = _parameterManager.NormalizeName( name );
        var result = _sqlParams.Find( t => t.Name?.ToUpperInvariant() == name?.ToUpperInvariant() );
        if ( result == null )
            return default;
        return Util.Helpers.Convert.To<T>( result.Value );
    }

    /// <summary>
    /// Obtiene la representación SQL para depuración, reemplazando los parámetros con sus valores literales.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL con los parámetros reemplazados por sus valores.
    /// </returns>
    /// <remarks>
    /// Este método es útil para visualizar la consulta SQL final que se ejecutará, permitiendo así una mejor depuración
    /// y análisis de la consulta generada.
    /// </remarks>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetParams"/>
    public virtual string GetDebugSql() {
        var sql = GetSql();
        var parameters = GetParams();
        foreach ( var parameter in parameters )
            sql = Regex.Replace( sql, $@"{parameter.Name}\b", _resolver.GetParamLiterals( parameter.Value ) );
        return sql;
    }
}
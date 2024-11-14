namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Clase base abstracta que representa una cláusula.
/// </summary>
/// <remarks>
/// Esta clase proporciona la estructura fundamental para las cláusulas derivadas,
/// permitiendo la implementación de comportamientos específicos en las subclases.
/// </remarks>
public abstract class ClauseBase {
    protected readonly SqlBuilderBase SqlBuilder;
    protected readonly IDialect Dialect;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ClauseBase"/>.
    /// </summary>
    /// <param name="sqlBuilder">Una instancia de <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="sqlBuilder"/> es <c>null</c>.</exception>
    protected ClauseBase( SqlBuilderBase sqlBuilder ) 
    { 
        SqlBuilder = sqlBuilder ?? throw new ArgumentNullException( nameof( sqlBuilder ) ); 
        Dialect = SqlBuilder.Dialect; 
    }

    /// <summary>
    /// Reemplaza el SQL en bruto utilizando el dialecto configurado.
    /// </summary>
    /// <param name="sql">La cadena de SQL en bruto que se desea reemplazar.</param>
    /// <returns>Una cadena que representa el SQL modificado según el dialecto.</returns>
    protected string ReplaceRawSql(string sql) {
        return Dialect.ReplaceSql(sql);
    }
}
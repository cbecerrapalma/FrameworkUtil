using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que representa un generador de sentencias SQL específico para Oracle.
/// Hereda de <see cref="SqlBuilderBase"/>.
/// </summary>
public class OracleSqlBuilder : SqlBuilderBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleSqlBuilder"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros de la consulta. Si se proporciona un valor nulo, se utilizará el administrador de parámetros predeterminado.</param>
    public OracleSqlBuilder( IParameterManager parameterManager = null )
        : base( parameterManager ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia del dialecto específico para Oracle.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDialect"/> que representa el dialecto de Oracle.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para obtener el dialecto adecuado para la interacción con bases de datos Oracle.
    /// </remarks>
    protected override IDialect CreateDialect() {
        return OracleDialect.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de caché de columnas específica para Oracle.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IColumnCache"/> que representa la caché de columnas de Oracle.
    /// </returns>
    /// <remarks>
    /// Este método anula el método base para proporcionar una implementación específica
    /// que utiliza la caché de columnas de Oracle.
    /// </remarks>
    protected override IColumnCache CreateColumnCache() {
        return OracleColumnCache.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una nueva instancia de un generador de SQL específico para Oracle.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="OracleSqlBuilder"/> que implementa <see cref="ISqlBuilder"/>.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica para Oracle.
    /// </remarks>
    public override ISqlBuilder New() {
        return new OracleSqlBuilder( ParameterManager );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IParameterManager"/> específica para el manejo de parámetros.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="OracleParameterManager"/> configurada con el dialecto actual.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para inicializar el gestor de parámetros que será utilizado en las operaciones de base de datos.
    /// </remarks>
    protected override IParameterManager CreateParameterManager() {
        return new OracleParameterManager( Dialect );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="OracleSqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="Clone"/> de la clase base.
    /// Se utiliza para crear una nueva instancia de <see cref="OracleSqlBuilder"/> 
    /// que contiene una copia de los datos del objeto original.
    /// </remarks>
    public override ISqlBuilder Clone() {
        var result = new OracleSqlBuilder();
        result.Clone( this );
        return result;
    }
}
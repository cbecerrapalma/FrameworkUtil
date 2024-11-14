using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que representa un generador de consultas SQL específico para PostgreSQL.
/// Hereda de <see cref="SqlBuilderBase"/> para proporcionar funcionalidades específicas de PostgreSQL.
/// </summary>
public class PostgreSqlBuilder : SqlBuilderBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlBuilder"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará. Si se pasa como <c>null</c>, se utilizará el comportamiento predeterminado.</param>
    public PostgreSqlBuilder( IParameterManager parameterManager = null )
        : base( parameterManager ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia del dialecto específico para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el dialecto de PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para obtener el dialecto adecuado que se debe usar 
    /// para interactuar con una base de datos PostgreSQL. 
    /// </remarks>
    protected override IDialect CreateDialect() {
        return PostgreSqlDialect.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de caché de columnas específica para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IColumnCache"/> que representa el caché de columnas de PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica de caché de columnas
    /// que es adecuada para el manejo de datos en una base de datos PostgreSQL.
    /// </remarks>
    protected override IColumnCache CreateColumnCache() {
        return PostgreSqlColumnCache.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una nueva instancia de un generador SQL específico para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ISqlBuilder"/> que está configurada para trabajar con PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método anula el método base para proporcionar una implementación específica 
    /// que utiliza el administrador de parámetros actual.
    /// </remarks>
    public override ISqlBuilder New() {
        return new PostgreSqlBuilder( ParameterManager );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="PostgreSqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="Clone"/> de la clase base,
    /// asegurando que se realice una copia adecuada de las propiedades del objeto.
    /// </remarks>
    public override ISqlBuilder Clone() {
        var result = new PostgreSqlBuilder();
        result.Clone( this );
        return result;
    }
}
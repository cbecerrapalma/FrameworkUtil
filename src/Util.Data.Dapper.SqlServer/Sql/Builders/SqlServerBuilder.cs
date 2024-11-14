using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que representa un generador de consultas SQL específico para SQL Server.
/// Hereda de <see cref="SqlBuilderBase"/> y proporciona implementaciones específicas para la construcción de consultas.
/// </summary>
public class SqlServerBuilder : SqlBuilderBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerBuilder"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará. Si se proporciona <c>null</c>, se utilizará el valor predeterminado.</param>
    public SqlServerBuilder( IParameterManager parameterManager = null )
        : base( parameterManager ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia del dialecto SQL para SQL Server.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDialect"/> que representa el dialecto de SQL Server.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para proporcionar la implementación específica del dialecto
    /// que se utilizará para las operaciones de base de datos en SQL Server.
    /// </remarks>
    protected override IDialect CreateDialect() {
        return SqlServerDialect.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de caché de columnas específica para SQL Server.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IColumnCache"/> que representa la caché de columnas para SQL Server.
    /// </returns>
    /// <remarks>
    /// Este método anula el método base para proporcionar una implementación específica de la caché de columnas
    /// que se utilizará en el contexto de SQL Server.
    /// </remarks>
    protected override IColumnCache CreateColumnCache() {
        return SqlServerColumnCache.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una nueva instancia de un constructor SQL específico para SQL Server.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es un <see cref="SqlServerBuilder"/>.
    /// </returns>
    public override ISqlBuilder New() {
        return new SqlServerBuilder( ParameterManager );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="SqlServerBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="Clone"/> de la clase base,
    /// asegurando que se realice una clonación adecuada de las propiedades del objeto.
    /// </remarks>
    public override ISqlBuilder Clone() {
        var result = new SqlServerBuilder();
        result.Clone( this );
        return result;
    }
}
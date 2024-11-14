using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que representa un generador de consultas SQL específico para MySQL.
/// Hereda de <see cref="SqlBuilderBase"/> y proporciona implementaciones específicas
/// para la construcción de sentencias SQL compatibles con MySQL.
/// </summary>
public class MySqlBuilder : SqlBuilderBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlBuilder"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará. Si se pasa como <c>null</c>, se utilizará el valor predeterminado.</param>
    public MySqlBuilder( IParameterManager parameterManager = null )
        : base( parameterManager ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia del dialecto específico para MySQL.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDialect"/> que representa el dialecto de MySQL.
    /// </returns>
    /// <remarks>
    /// Este método anula el método base para proporcionar un dialecto específico
    /// que se utilizará en las operaciones de base de datos.
    /// </remarks>
    protected override IDialect CreateDialect() {
        return MySqlDialect.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de caché de columnas específica para MySQL.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IColumnCache"/> que representa la caché de columnas de MySQL.
    /// </returns>
    /// <remarks>
    /// Este método anula la implementación base para proporcionar una caché de columnas optimizada
    /// para el uso con bases de datos MySQL.
    /// </remarks>
    protected override IColumnCache CreateColumnCache() {
        return MySqlColumnCache.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una nueva instancia de un constructor SQL específico para MySQL.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que está configurada para MySQL.
    /// </returns>
    /// <remarks>
    /// Este método anula la implementación base para proporcionar un constructor SQL específico
    /// que utiliza el gestor de parámetros actual.
    /// </remarks>
    public override ISqlBuilder New() {
        return new MySqlBuilder( ParameterManager );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="MySqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="Clone"/> de la clase base.
    /// Se utiliza para crear un nuevo constructor de consultas SQL basado en el estado actual.
    /// </remarks>
    public override ISqlBuilder Clone() {
        var result = new MySqlBuilder();
        result.Clone( this );
        return result;
    }
}
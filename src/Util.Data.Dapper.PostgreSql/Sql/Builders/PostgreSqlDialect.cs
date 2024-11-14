using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa el dialecto específico para bases de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DialectBase"/> y proporciona implementaciones específicas
/// para las características y comportamientos de PostgreSQL.
/// </remarks>
public class PostgreSqlDialect : DialectBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlDialect"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor es privado y se utiliza para evitar la creación de instancias de la clase desde fuera.
    /// </remarks>
    private PostgreSqlDialect() {
    }

    public static readonly IDialect Instance = new PostgreSqlDialect();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de apertura.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de apertura, en este caso, un carácter de comillas dobles.
    /// </returns>
    /// <inheritdoc />
    public override string GetOpeningIdentifier() {
        return "\"";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de cierre para el elemento actual.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de cierre, en este caso, una comilla doble.
    /// </returns>
    /// <inheritdoc />
    public override string GetClosingIdentifier() {
        return "\"";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el prefijo asociado con la implementación actual.
    /// </summary>
    /// <returns>
    /// Un string que representa el prefijo, en este caso, el carácter "@".
    /// </returns>
    /// <inheritdoc />
    public override string GetPrefix() {
        return "@";
    }
}
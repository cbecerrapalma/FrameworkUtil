using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa un dialecto específico para la base de datos SQL Server.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DialectBase"/> y proporciona implementaciones específicas
/// para las operaciones de base de datos que son particulares de SQL Server.
/// </remarks>
public class SqlServerDialect : DialectBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerDialect"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor es privado y se utiliza para evitar la creación de instancias de la clase 
    /// <see cref="SqlServerDialect"/> desde fuera de la misma. 
    /// </remarks>
    private SqlServerDialect() {
    }

    public static readonly IDialect Instance = new SqlServerDialect();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de apertura para el formato específico.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de apertura, en este caso, un corchete izquierdo "[".
    /// </returns>
    /// <inheritdoc />
    public override string GetOpeningIdentifier() {
        return "[";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de cierre.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de cierre, en este caso, un corchete derecho.
    /// </returns>
    /// <inheritdoc />
    public override string GetClosingIdentifier() {
        return "]";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el prefijo utilizado por la clase derivada.
    /// </summary>
    /// <returns>
    /// Un string que representa el prefijo, en este caso, el carácter "@".
    /// </returns>
    /// <inheritdoc />
    public override string GetPrefix() {
        return "@";
    }
}
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa un dialecto específico para la base de datos MySQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DialectBase"/> y proporciona implementaciones específicas
/// para las operaciones y características del dialecto MySQL.
/// </remarks>
public class MySqlDialect : DialectBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlDialect"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor es privado y se utiliza para evitar la creación de instancias de la clase
    /// <see cref="MySqlDialect"/> desde fuera de la misma.
    /// </remarks>
    private MySqlDialect() {
    }

    public static readonly IDialect Instance = new MySqlDialect();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de apertura utilizado en la representación de cadenas.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de apertura, en este caso, un acento grave (`).
    /// </returns>
    /// <inheritdoc />
    public override string GetOpeningIdentifier() {
        return "`";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de cierre utilizado en la implementación.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de cierre, en este caso, un carácter de comillas invertidas.
    /// </returns>
    /// <inheritdoc />
    public override string GetClosingIdentifier() {
        return "`";
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
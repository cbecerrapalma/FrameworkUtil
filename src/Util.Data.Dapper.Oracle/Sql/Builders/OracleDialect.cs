using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Representa el dialecto específico para la base de datos Oracle.
/// Esta clase hereda de <see cref="DialectBase"/> y proporciona
/// implementaciones específicas para las operaciones y características
/// de la base de datos Oracle.
/// </summary>
public class OracleDialect : DialectBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleDialect"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor es privado y se utiliza para evitar la creación de instancias de la clase 
    /// <see cref="OracleDialect"/> desde fuera de la misma. 
    /// </remarks>
    private OracleDialect() {
    }

    public static readonly IDialect Instance = new OracleDialect();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de apertura para un formato específico.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de apertura, en este caso, una comilla doble.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe la implementación base para proporcionar un identificador de apertura específico.
    /// </remarks>
    public override string GetOpeningIdentifier() {
        return "\"";
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador de cierre.
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
    /// Obtiene el prefijo utilizado por la clase derivada.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el prefijo, en este caso, ":".
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar un prefijo específico.
    /// </remarks>
    public override string GetPrefix() {
        return ":";
    }
}
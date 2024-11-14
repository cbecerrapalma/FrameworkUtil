using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Dapper.Sql.Builders;

/// <summary>
/// Clase que gestiona los parámetros específicos para la conexión a una base de datos Oracle.
/// Hereda de la clase <see cref="ParameterManager"/>.
/// </summary>
public class OracleParameterManager : ParameterManager {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleParameterManager"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de la base de datos que se utilizará para la gestión de parámetros.</param>
    public OracleParameterManager( IDialect dialect ) : base( dialect ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleParameterManager"/>.
    /// </summary>
    /// <param name="manager">El objeto <see cref="ParameterManager"/> que se utilizará para la gestión de parámetros.</param>
    public OracleParameterManager( ParameterManager manager ) : base( manager ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Genera un nombre único para un parámetro utilizando un prefijo del dialecto actual.
    /// </summary>
    /// <returns>
    /// Un string que representa el nombre generado para el parámetro.
    /// </returns>
    /// <remarks>
    /// Este método incrementa el índice del parámetro cada vez que se llama, asegurando que cada nombre generado sea único.
    /// </remarks>
    public override string GenerateName() {
        var result = $"{Dialect.GetPrefix()}p_{ParamIndex}";
        ParamIndex++;
        return result;
    }
}
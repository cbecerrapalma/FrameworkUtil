using Util.Data.Metadata;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Clase que implementa la conversión de tipos para PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase se encarga de convertir tipos de datos de C# a tipos de datos de PostgreSQL 
/// y viceversa, facilitando la interacción con bases de datos PostgreSQL.
/// </remarks>
public class PostgreSqlTypeConverter : ITypeConverter {
    /// <inheritdoc />
    /// <summary>
    /// Convierte un tipo de dato de cadena a un tipo de dato de base de datos.
    /// </summary>
    /// <param name="dataType">El tipo de dato en forma de cadena que se desea convertir.</param>
    /// <param name="length">La longitud opcional del tipo de dato, si aplica.</param>
    /// <returns>Un valor nullable de <see cref="DbType"/> que representa el tipo de dato convertido, o null si el tipo de dato es vacío.</returns>
    /// <exception cref="NotImplementedException">Se lanza si el tipo de dato no es reconocido.</exception>
    /// <remarks>
    /// Este método realiza una conversión de varios tipos de datos comunes a sus equivalentes en <see cref="DbType"/>.
    /// Los tipos de datos soportados incluyen: "uuid", "varchar", "text", "json", "jsonb", "xml", "bool", "char",
    /// "int2", "int4", "int8", "float4", "float8", "numeric", "decimal", "date", "time", "timetz", "timestamp",
    /// "timestamptz" y "bytea".
    /// </remarks>
    public DbType? ToType( string dataType, int? length = null ) {
        if ( dataType.IsEmpty() )
            return null;
        switch ( dataType.ToLower() ) {
            case "uuid":
                return DbType.Guid;
            case "varchar":
            case "text":
            case "json":
            case "jsonb":
            case "xml":
                return DbType.String;
            case "bool":
                return DbType.Boolean;
            case "char":
                return DbType.Byte;
            case "int2":
                return DbType.Int16;
            case "int4":
                return DbType.Int32;
            case "int8":
                return DbType.Int64;
            case "float4":
                return DbType.Single;
            case "float8":
                return DbType.Double;
            case "numeric":
            case "decimal":
                return DbType.Decimal;
            case "date":
                return DbType.Date;
            case "time":
            case "timetz":
                return DbType.Time;
            case "timestamp":
            case "timestamptz":
                return DbType.DateTime;
            case "bytea":
                return DbType.Binary;
        }
        throw new NotImplementedException( dataType );
    }
}
using Util.Data.Metadata;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Clase que implementa la conversión de tipos específicos para MySQL.
/// </summary>
/// <remarks>
/// Esta clase es responsable de convertir tipos de datos entre el formato de MySQL y el formato utilizado en la aplicación.
/// </remarks>
public class MySqlTypeConverter : ITypeConverter {
    /// <inheritdoc />
    /// <summary>
    /// Convierte un tipo de dato de cadena a un tipo de dato de base de datos.
    /// </summary>
    /// <param name="dataType">El tipo de dato en forma de cadena que se desea convertir.</param>
    /// <param name="length">La longitud opcional del tipo de dato, utilizada en algunos casos específicos.</param>
    /// <returns>
    /// Un valor nullable de <see cref="DbType"/> que representa el tipo de dato correspondiente,
    /// o null si el tipo de dato proporcionado está vacío.
    /// </returns>
    /// <exception cref="NotImplementedException">
    /// Se lanza si el tipo de dato proporcionado no está implementado en la conversión.
    /// </exception>
    /// <remarks>
    /// Este método realiza la conversión de varios tipos de datos comunes de bases de datos
    /// a sus equivalentes en el tipo <see cref="DbType"/>. Se consideran casos específicos
    /// como la longitud de ciertos tipos de datos, como "char" y "tinyint".
    /// </remarks>
    public DbType? ToType( string dataType, int? length = null ) {
        if ( dataType.IsEmpty() )
            return null;
        switch ( dataType.ToLower() ) {
            case "char":
                return length == 36 ? DbType.Guid : DbType.String;
            case "varchar":
            case "tinytext":
            case "mediumtext":
            case "longtext":
            case "text":
                return DbType.String;
            case "tinyint":
                return length == 1 ? DbType.Boolean : DbType.Byte;
            case "bit":
                return DbType.Boolean;
            case "smallint":
                return DbType.Int16;
            case "integer":
            case "int":
            case "mediumint":
                return DbType.Int32;
            case "bigint":
                return DbType.Int64;
            case "float":
                return DbType.Single;
            case "double":
                return DbType.Double;
            case "decimal":
            case "numeric":
                return DbType.Decimal;
            case "date":
                return DbType.Date;
            case "time":
                return DbType.Time;
            case "datetime":
            case "timestamp":
                return DbType.DateTime;
            case "tinyblob":
            case "mediumblob":
            case "longblob":
            case "blob":
                return DbType.Binary;
        }
        throw new NotImplementedException( dataType );
    }
}
using Util.Data.Metadata;

namespace Util.Data.Dapper.Metadata {
    /// <summary>
    /// Clase que implementa la conversión de tipos específicos para SQL Server.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz <see cref="ITypeConverter"/> y proporciona
    /// métodos para convertir entre tipos de datos de .NET y tipos de datos de SQL Server.
    /// </remarks>
    public class SqlServerTypeConverter : ITypeConverter {
        /// <inheritdoc />
        /// <summary>
        /// Convierte un tipo de dato de cadena a un tipo de dato de base de datos (DbType).
        /// </summary>
        /// <param name="dataType">El tipo de dato en forma de cadena que se desea convertir.</param>
        /// <param name="length">La longitud opcional del tipo de dato, si es aplicable.</param>
        /// <returns>
        /// Un valor de tipo <see cref="DbType"/> correspondiente al tipo de dato proporcionado,
        /// o <c>null</c> si el tipo de dato está vacío.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Se lanza si el tipo de dato proporcionado no es reconocido.
        /// </exception>
        /// <remarks>
        /// Este método realiza la conversión de varios tipos de datos comunes de bases de datos
        /// a sus equivalentes en el enumerado <see cref="DbType"/>. Si el tipo de dato no es
        /// reconocido, se lanza una excepción.
        /// </remarks>
        public DbType? ToType( string dataType, int? length = null ) {
            if ( dataType.IsEmpty() )
                return null;
            switch ( dataType.ToLower() ) {
                case "uniqueidentifier":
                    return DbType.Guid;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "nchar":
                    return DbType.StringFixedLength;
                case "varchar":
                    return DbType.AnsiString;
                case "nvarchar":
                case "text":
                case "ntext":
                    return DbType.String;
                case "bit":
                    return DbType.Boolean;
                case "tinyint":
                    return DbType.Byte;
                case "smallint":
                    return DbType.Int16;
                case "int":
                    return DbType.Int32;
                case "bigint":
                    return DbType.Int64;
                case "real":
                    return DbType.Single;
                case "float":
                    return DbType.Double;
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    return DbType.Decimal;
                case "date":
                    return DbType.Date;
                case "time":
                    return DbType.Time;
                case "datetime":
                case "smalldatetime":
                    return DbType.DateTime;
                case "datetime2":
                    return DbType.DateTime2;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "binary":
                case "varbinary":
                case "varbinary(max)":
                case "image":
                case "rowversion":
                case "timestamp":
                    return DbType.Binary;
                case "xml":
                    return DbType.Xml;
                case "sql_variant":
                    return DbType.Object;
            }
            throw new NotImplementedException( dataType );
        }
    }
}
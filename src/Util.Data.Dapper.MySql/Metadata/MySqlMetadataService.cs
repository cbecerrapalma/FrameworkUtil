using Util.Data.Metadata;
using Util.Data.Sql;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Clase que implementa el servicio de metadatos para MySQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos para acceder y manipular metadatos en una base de datos MySQL.
/// </remarks>
public class MySqlMetadataService : IMetadataService {
    private readonly ISqlQuery _sqlQuery;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlMetadataService"/>.
    /// </summary>
    /// <param name="sqlQuery">Una instancia de <see cref="ISqlQuery"/> que se utilizará para realizar consultas SQL.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="sqlQuery"/> es <c>null</c>.</exception>
    public MySqlMetadataService( ISqlQuery sqlQuery ) {
        _sqlQuery = sqlQuery ?? throw new ArgumentNullException( nameof( sqlQuery ) );
    }

    /// <summary>
    /// Obtiene información de la base de datos de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado de tipo <see cref="DatabaseInfo"/> que contiene la información de la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método llama a otros métodos asíncronos para obtener la información de la base de datos y las tablas, 
    /// y luego agrega las tablas obtenidas al resultado de la base de datos.
    /// </remarks>
    public async Task<DatabaseInfo> GetDatabaseInfoAsync() {
        var result = await GetDatabase();
        var tables = await GetTables();
        result.Tables.AddRange( tables );
        return result;
    }

    /// <summary>
    /// Obtiene información sobre la base de datos actual.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, con un resultado de tipo <see cref="DatabaseInfo"/> que contiene el ID y el nombre de la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una consulta SQL para obtener el ID y el nombre de la base de datos actual.
    /// </remarks>
    private async Task<DatabaseInfo> GetDatabase() {
        return await _sqlQuery
            .AppendSelect( "Database() [Id],Database() [Name]" )
            .ToEntityAsync<DatabaseInfo>();
    }

    /// <summary>
    /// Obtiene una lista de información sobre las tablas desde la base de datos.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado de la tarea es una lista de objetos <see cref="TableInfo"/> que contienen información sobre las tablas.
    /// </returns>
    /// <remarks>
    /// Este método establece primero la consulta SQL necesaria para obtener la información de las tablas y luego ejecuta la consulta de forma asincrónica.
    /// Se utiliza un diccionario para evitar duplicados y asociar columnas a sus respectivas tablas.
    /// </remarks>
    private async Task<List<TableInfo>> GetTables() {
        SetGetTablesSql();
        var dic = new Dictionary<string, TableInfo>();
        var tables = ( await _sqlQuery.ToListAsync<TableInfo, ColumnInfo, TableInfo>( ( table, column ) => {
            if ( table == null || column == null )
                return null;
            dic.TryAdd( table.Id, table );
            TableInfo result = dic[table.Id];
            result.Columns.Add( column );
            return result;
        } ) ).Distinct().ToList();
        return tables;
    }

    /// <summary>
    /// Configura la consulta SQL para obtener información sobre las tablas y columnas de la base de datos.
    /// </summary>
    /// <remarks>
    /// Este método construye una consulta SQL que selecciona el nombre de la tabla, el nombre de la columna,
    /// el comentario de la columna, el tipo de dato, la precisión numérica, la escala, y otros atributos
    /// relevantes de las columnas, como si son claves primarias, si son auto-incrementales y si son nulas.
    /// </remarks>
    private void SetGetTablesSql() {
        _sqlQuery
            .Select( "t.Table_Name Id,t.Table_Name Name,t.Table_Comment Comment" )
            .Select( "c.Column_Name Id,c.Column_Name Name,c.Column_Comment Comment" )
            .Select( "c.Data_Type DataType,c.Numeric_Precision Precision,c.Numeric_Scale Scale" )
            .AppendSelect( ",( Case When c.Column_Key = 'PRI' Then 1 Else 0 End ) IsPrimaryKey" )
            .AppendSelect( ",( Case When c.Extra = 'auto_increment' Then 1 Else 0 End ) AS IsAutoIncrement" )
            .AppendSelect( ",( Case When c.Is_Nullable = 'NO' Then 0 Else 1 End ) AS IsNullable" )
            .AppendSelect( ",( Case When c.Column_Type = 'tinyint(1)' Then 1 " )
            .AppendSelect( "When c.Numeric_Precision Is Not Null Then c.Numeric_Precision " )
            .AppendSelect( "When c.Character_Maximum_Length Is Not Null Then c.Character_Maximum_Length Else Null End) Length" )
            .From( "Information_Schema.Tables t" )
            .Join( "Information_Schema.Columns c" ).On( "t.Table_Schema", "c.Table_Schema" ).On( "t.Table_Name", "c.Table_Name" )
            .AppendWhere( "[t].[Table_Schema] = Database()" );
    }
}
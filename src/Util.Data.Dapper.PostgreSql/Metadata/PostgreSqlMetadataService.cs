using Util.Data.Metadata;
using Util.Data.Sql;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Proporciona servicios de metadatos para una base de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IMetadataService"/> y ofrece métodos para 
/// interactuar con los metadatos de una base de datos PostgreSQL.
/// </remarks>
public class PostgreSqlMetadataService : IMetadataService {
    private readonly ISqlQuery _sqlQuery;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlMetadataService"/>.
    /// </summary>
    /// <param name="sqlQuery">Una instancia de <see cref="ISqlQuery"/> que se utilizará para ejecutar consultas SQL.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="sqlQuery"/> es <c>null</c>.</exception>
    public PostgreSqlMetadataService( ISqlQuery sqlQuery ) {
        _sqlQuery = sqlQuery ?? throw new ArgumentNullException( nameof( sqlQuery ) );
    }

    /// <summary>
    /// Obtiene información sobre la base de datos de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es un objeto <see cref="DatabaseInfo"/> que contiene información sobre la base de datos, incluyendo una lista de tablas.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="GetDatabase"/> para obtener la información básica de la base de datos y a <see cref="GetTables"/> para obtener las tablas asociadas. 
    /// Luego, agrega las tablas obtenidas al objeto de información de la base de datos antes de devolverlo.
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
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es un objeto <see cref="DatabaseInfo"/> que contiene la información de la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método realiza una consulta a la base de datos para seleccionar el identificador y el nombre de la base de datos actual.
    /// </remarks>
    private async Task<DatabaseInfo> GetDatabase() {
        return await _sqlQuery
            .Select( "oid Id,datname Name" )
            .From( "pg_database" )
            .AppendWhere( "datname=current_database()" )
            .ToEntityAsync<DatabaseInfo>();
    }

    /// <summary>
    /// Obtiene una lista de tablas junto con su información de columnas desde la base de datos.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de objetos <see cref="TableInfo"/> 
    /// que representan las tablas y sus respectivas columnas.
    /// </returns>
    /// <remarks>
    /// Este método establece primero la consulta SQL necesaria para obtener la información de las tablas. 
    /// Luego, utiliza un diccionario para almacenar las tablas y sus columnas, asegurando que cada tabla 
    /// se agregue solo una vez a la lista final.
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
    /// Configura la consulta SQL para obtener información sobre las tablas en la base de datos.
    /// </summary>
    /// <remarks>
    /// Este método construye una consulta SQL que selecciona el identificador, el nombre y el esquema de las tablas,
    /// así como los comentarios asociados a cada tabla. También se une a la tabla de columnas para obtener información adicional.
    /// </remarks>
    private void SetGetTablesSql() {
        _sqlQuery
            .Select( "c1.oid Id,t.tablename Name,t.schemaname Schema" )
            .AppendSelect( ",obj_description(c1.oid) [Comment]" )
            .Select( "c.*" )
            .From( "pg_tables t" )
            .Join( "pg_class c1" ).On( "t.tablename", "c1.relname" )
            .Join( GetColumnsSql(), "c" ).On( "c.table_id", "c1.oid" )
            .In( "t.schemaname", t => t
                .Select( "schema_name" )
                .From( "information_schema.schemata" )
                .AppendWhere( "catalog_name=current_database()" )
                .AppendWhere( "schema_name != 'information_schema'" )
                .AppendWhere( "schema_name not like 'pg_%'" )
            );
    }

    /// <summary>
    /// Obtiene la construcción SQL para seleccionar columnas de una tabla en la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ISqlBuilder"/> que contiene la consulta SQL construida para obtener información sobre las columnas.
    /// </returns>
    /// <remarks>
    /// Esta función construye una consulta SQL que selecciona varios atributos de las columnas de una tabla, incluyendo 
    /// el identificador de la tabla, el nombre de la columna, el tipo de dato, la precisión, la escala, la longitud, 
    /// si es nullable, si es clave primaria, si es autoincremental y un comentario sobre la columna.
    /// </remarks>
    private ISqlBuilder GetColumnsSql() {
        return _sqlQuery.NewSqlBuilder()
            .Select( "a.attrelid table_id,a.attname Id,a.attname Name,col.udt_name DataType" )
            .Select( "col.numeric_precision Precision,col.numeric_scale Scale" )
            .AppendSelect( ",Coalesce(col.character_maximum_length, col.numeric_precision, -1) [Length]" )
            .AppendSelect( ",( Case a.attnotnull When 't' Then 0 Else 1 End ) [IsNullable]" )
            .AppendSelect( ",( Case a.attnum When con.conkey[[1]] Then 1 Else 0 End ) [IsPrimaryKey]" )
            .AppendSelect( ",( Case When col.is_identity='YES' Then 1 Else 0 End ) [IsAutoIncrement]" )
            .AppendSelect( ",col_description( a.attrelid, a.attnum ) [Comment]" )
            .From( "pg_attribute a" )
            .Join( "pg_class c2" ).On( "a.attrelid", "c2.oid" )
            .Join( "pg_constraint con" ).On( "con.conrelid", "c2.oid" ).AppendOn( "con.contype = 'p'" )
            .Join( "pg_namespace n" ).On( "n.oid", "c2.relnamespace" )
            .Join( "information_schema.columns col" ).On( "col.table_schema", "n.nspname" ).On( "col.table_name", "c2.relname" ).On( "col.column_name", "a.attname" );
    }
}
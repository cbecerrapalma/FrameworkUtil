using Util.Data.Metadata;
using Util.Data.Sql;

namespace Util.Data.Dapper.Metadata {
    /// <summary>
    /// Proporciona servicios de metadatos para una base de datos SQL Server.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz <see cref="IMetadataService"/> y ofrece métodos para 
    /// recuperar y manipular metadatos relacionados con las tablas, columnas y otros objetos 
    /// dentro de una base de datos SQL Server.
    /// </remarks>
    public class SqlServerMetadataService : IMetadataService {
        private readonly ISqlQuery _sqlQuery;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SqlServerMetadataService"/>.
        /// </summary>
        /// <param name="sqlQuery">Una instancia de <see cref="ISqlQuery"/> que se utilizará para ejecutar consultas SQL.</param>
        /// <exception cref="ArgumentNullException">Se lanza si <paramref name="sqlQuery"/> es <c>null</c>.</exception>
        public SqlServerMetadataService( ISqlQuery sqlQuery ) {
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException( nameof( sqlQuery ) );
        }

        /// <summary>
        /// Obtiene información sobre la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona, 
        /// que contiene un objeto <see cref="DatabaseInfo"/> con la información de la base de datos.
        /// </returns>
        /// <remarks>
        /// Este método llama a otros métodos asíncronos para obtener la información de la base de datos 
        /// y las tablas, y luego agrega las tablas al resultado antes de devolverlo.
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
        /// Una tarea que representa la operación asincrónica. El valor de la tarea contiene un objeto <see cref="DatabaseInfo"/> 
        /// que representa la información de la base de datos.
        /// </returns>
        /// <remarks>
        /// Este método realiza una consulta a la base de datos para obtener el identificador y el nombre de la base de datos 
        /// actual utilizando la tabla <c>sys.SysDataBases</> y filtrando por el <c>DbId</> correspondiente al proceso actual.
        /// </remarks>
        private async Task<DatabaseInfo> GetDatabase() {
            return await _sqlQuery
                .Select( "Dbid Id,Name" )
                .From( "sys.SysDataBases" )
                .Where( "DbId", t => t.Select( "Dbid" ).From( "sys.SysProcesses" ).AppendWhere( "Spid = @@spid" ) )
                .ToEntityAsync<DatabaseInfo>();
        }

        /// <summary>
        /// Obtiene una lista de tablas y sus columnas asociadas desde la base de datos.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona, que contiene una lista de objetos <see cref="TableInfo"/> 
        /// que representan las tablas y sus columnas.
        /// </returns>
        /// <remarks>
        /// Este método establece primero la consulta SQL necesaria para obtener las tablas y columnas. 
        /// Luego, utiliza un diccionario para almacenar las tablas y sus respectivas columnas, asegurando 
        /// que cada tabla se agregue solo una vez a la lista final.
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
        /// Este método construye una consulta SQL que selecciona varios atributos de las tablas, 
        /// incluyendo el identificador, el nombre, el esquema, los comentarios, y si la columna es una clave primaria.
        /// También incluye información sobre si las columnas son autoincrementales y si son anulables.
        /// </remarks>
        private void SetGetTablesSql() {
            _sqlQuery
                .Select( "o.object_id Id,o.name,s.name Schema,ep.value Comment" )
                .Select( "c.Id,c.name,c.Comment" )
                .AppendSelect( ",(Case When Exists(" )
                .AppendSelect( GetIsPrimaryKeySql() )
                .AppendSelect( ") Then Cast(1 As Bit) Else Cast(0 As Bit) End) As IsPrimaryKey" )
                .Select( "c.is_identity IsAutoIncrement,c.is_nullable IsNullable" )
                .Select( "c.DataType,c.max_length Length,c.precision,c.scale" )
                .From( "sys.Objects o" )
                .LeftJoin( "sys.Schemas s" ).On( "o.schema_id", "s.schema_id" )
                .LeftJoin( "sys.Extended_Properties ep" ).On( "o.object_id", "ep.major_id" ).On( "ep.minor_id", 0 )
                .Join( GetColumnsSql(), "c" ).On( "c.object_id", "o.object_id" )
                .In( "o.type", new[] { 'U' } );
        }

        /// <summary>
        /// Obtiene un objeto ISqlBuilder configurado para consultar las columnas que son claves primarias en la base de datos.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="ISqlBuilder"/> que contiene la consulta SQL para obtener las columnas de clave primaria.
        /// </returns>
        /// <remarks>
        /// Este método construye una consulta SQL que selecciona las columnas de clave primaria de la tabla 
        /// "Information_Schema.Key_Column_Usage" y se une a "Information_Schema.Table_Constraints" 
        /// para filtrar las restricciones de tipo 'PRIMARY KEY'. Además, se aplican condiciones adicionales 
        /// para asegurar que la consulta se limite a las tablas y columnas específicas.
        /// </remarks>
        private ISqlBuilder GetIsPrimaryKeySql() {
            return _sqlQuery.NewSqlBuilder()
                .Select()
                .From( "Information_Schema.Key_Column_Usage k" )
                .Join( "Information_Schema.Table_Constraints tc" )
                .On( "k.table_name", "tc.table_name" )
                .On( "k.Constraint_Name", "tc.Constraint_Name" )
                .AppendOn( "tc.Constraint_Type='PRIMARY KEY'" )
                .AppendWhere( "o.name=k.table_name" )
                .AppendWhere( "c.name=k.column_name" );
        }

        /// <summary>
        /// Obtiene un objeto <see cref="ISqlBuilder"/> que representa una consulta SQL 
        /// para seleccionar columnas de una base de datos, incluyendo información adicional 
        /// como comentarios y tipos de datos.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="ISqlBuilder"/> que contiene la consulta SQL construida 
        /// para obtener las columnas y sus propiedades asociadas.
        /// </returns>
        /// <remarks>
        /// Esta función construye una consulta que selecciona varios atributos de las 
        /// columnas en la tabla 'sys.Columns', así como propiedades extendidas de las 
        /// columnas y tipos de datos de la tabla 'sys.Types'. Se excluyen las columnas 
        /// cuyo tipo de dato es 'sysname'.
        /// </remarks>
        private ISqlBuilder GetColumnsSql() {
            return _sqlQuery.NewSqlBuilder()
                .Select( "c.object_id,c.column_id Id,c.name,ep.value Comment" )
                .Select( "c.is_identity,c.is_nullable,t.name DataType,c.max_length" )
                .Select( "c.precision,c.scale" )
                .From( "sys.Columns c" )
                .LeftJoin( "sys.Extended_Properties ep" ).On( "c.object_id", "ep.major_id" ).On( "c.column_id", "ep.minor_id" )
                .LeftJoin( "sys.Types t" ).On( "c.system_type_id", "t.system_type_id" )
                .AppendWhere( "t.name!='sysname'" );
        }
    }
}
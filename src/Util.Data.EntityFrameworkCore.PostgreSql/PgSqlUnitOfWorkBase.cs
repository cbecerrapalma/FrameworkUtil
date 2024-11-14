namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Clase base abstracta para la implementación de la unidad de trabajo en PostgreSQL.
/// Hereda de <see cref="UnitOfWorkBase"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para gestionar la transacción y el contexto de la base de datos 
/// en aplicaciones que utilizan PostgreSQL como sistema de gestión de bases de datos.
/// </remarks>
public abstract class PgSqlUnitOfWorkBase : UnitOfWorkBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PgSqlUnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán para configurar el contexto.</param>
    protected PgSqlUnitOfWorkBase( IServiceProvider serviceProvider, DbContextOptions options )
        : base( serviceProvider, options ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cadena de conexión para el inquilino utilizando Npgsql.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de base de datos.</param>
    /// <param name="connectionString">La cadena de conexión a utilizar para conectarse a la base de datos.</param>
    /// <inheritdoc />
    protected override void ConfigTenantConnectionString( DbContextOptionsBuilder optionsBuilder, string connectionString ) {
        optionsBuilder.UseNpgsql( connectionString );
    }
}
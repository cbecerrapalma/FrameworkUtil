namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Clase base abstracta para implementar un patrón de Unidad de Trabajo específico para SQLite.
/// </summary>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para gestionar las transacciones y el contexto de la base de datos SQLite.
/// </remarks>
public abstract class SqliteUnitOfWorkBase : UnitOfWorkBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqliteUnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán para la conexión.</param>
    protected SqliteUnitOfWorkBase( IServiceProvider serviceProvider, DbContextOptions options )
        : base( serviceProvider, options ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cadena de conexión para el inquilino utilizando SQLite.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de base de datos.</param>
    /// <param name="connectionString">La cadena de conexión que se utilizará para conectarse a la base de datos SQLite.</param>
    /// <remarks>
    /// Este método se sobrescribe para proporcionar una implementación específica que utiliza SQLite como proveedor de base de datos.
    /// </remarks>
    protected override void ConfigTenantConnectionString( DbContextOptionsBuilder optionsBuilder, string connectionString ) {
        optionsBuilder.UseSqlite( connectionString );
    }
}
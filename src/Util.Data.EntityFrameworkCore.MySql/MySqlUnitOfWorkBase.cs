namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Clase base abstracta para la implementación de un patrón Unit of Work específico para MySQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona una base para gestionar las transacciones y la persistencia de datos en una base de datos MySQL.
/// Debe ser heredada por clases concretas que implementen la lógica específica de acceso a datos.
/// </remarks>
public abstract class MySqlUnitOfWorkBase : UnitOfWorkBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlUnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán para la configuración del contexto.</param>
    protected MySqlUnitOfWorkBase( IServiceProvider serviceProvider, DbContextOptions options )
        : base( serviceProvider, options ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cadena de conexión del inquilino para el contexto de la base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de la base de datos.</param>
    /// <param name="connectionString">La cadena de conexión que se utilizará para conectarse a la base de datos.</param>
    /// <remarks>
    /// Este método utiliza MySQL como proveedor de base de datos y detecta automáticamente la versión del servidor 
    /// a partir de la cadena de conexión proporcionada.
    /// </remarks>
    protected override void ConfigTenantConnectionString( DbContextOptionsBuilder optionsBuilder, string connectionString ) {
        optionsBuilder.UseMySql( connectionString, ServerVersion.AutoDetect( connectionString ) );
    }
}
using Util.Domain;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Clase base abstracta que representa una unidad de trabajo para SQL Server.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación base para manejar transacciones y operaciones de base de datos
/// específicas de SQL Server. Debe ser heredada por clases concretas que implementen la lógica de negocio
/// y las operaciones de acceso a datos.
/// </remarks>
public abstract class SqlServerUnitOfWorkBase : UnitOfWorkBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerUnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán para la configuración del contexto.</param>
    protected SqlServerUnitOfWorkBase(IServiceProvider serviceProvider, DbContextOptions options)
        : base(serviceProvider, options) { }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cadena de conexión del inquilino para el contexto de la base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de la base de datos.</param>
    /// <param name="connectionString">La cadena de conexión que se utilizará para conectarse a la base de datos.</param>
    /// <remarks>
    /// Este método anula la implementación base para establecer la conexión a la base de datos SQL Server
    /// utilizando la cadena de conexión proporcionada.
    /// </remarks>
    protected override void ConfigTenantConnectionString(DbContextOptionsBuilder optionsBuilder, string connectionString) 
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    /// <inheritdoc />
    /// <summary>
    /// Aplica la configuración de versión a un tipo de entidad en el modelo.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para construir el modelo.</param>
    /// <param name="entityType">El tipo de entidad <see cref="IMutableEntityType"/> al que se le aplicará la configuración.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="IVersion"/>.
    /// Si es así, se configura la propiedad "Version" como una columna de tipo row version en la base de datos.
    /// </remarks>
    protected override void ApplyVersion(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (typeof(IVersion).IsAssignableFrom(entityType.ClrType) == false)
            return;
        modelBuilder.Entity(entityType.ClrType)
            .Property("Version")
            .HasColumnName("Version")
            .IsRowVersion();
    }

    /// <summary>
    /// Obtiene la versión del objeto actual.
    /// </summary>
    /// <returns>
    /// Un arreglo de bytes que representa la versión, o null si no hay versión disponible.
    /// </returns>
    protected override byte[] GetVersion() {
        return null;
    }
}
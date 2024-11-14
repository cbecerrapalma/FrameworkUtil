using Microsoft.Extensions.Options;
using Util.Data.EntityFrameworkCore.ValueComparers;
using Util.Data.EntityFrameworkCore.ValueConverters;
using Util.Domain.Extending;
using Util.Properties;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Clase abstracta que representa una unidad de trabajo base para la implementación de patrones de repositorio
/// en una base de datos Oracle.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="UnitOfWorkBase"/> y proporciona funcionalidades específicas
/// para manejar transacciones y operaciones en una base de datos Oracle.
/// </remarks>
public abstract class OracleUnitOfWorkBase : UnitOfWorkBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleUnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán para configurar el contexto.</param>
    protected OracleUnitOfWorkBase( IServiceProvider serviceProvider, DbContextOptions options )
        : base( serviceProvider, options ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cadena de conexión para el inquilino utilizando Oracle como proveedor de base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El objeto <see cref="DbContextOptionsBuilder"/> que se utilizará para configurar el contexto de la base de datos.</param>
    /// <param name="connectionString">La cadena de conexión que se utilizará para conectarse a la base de datos Oracle.</param>
    /// <remarks>
    /// Este método anula la implementación base para establecer la conexión a la base de datos específica del inquilino.
    /// </remarks>
    protected override void ConfigTenantConnectionString( DbContextOptionsBuilder optionsBuilder, string connectionString ) {
        optionsBuilder.UseOracle( connectionString );
    }

    /// <summary>
    /// Aplica propiedades adicionales a la entidad especificada en el modelo.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad <see cref="IMutableEntityType"/> al que se le aplicarán las propiedades adicionales.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="IExtraProperties"/>.
    /// Si es así, se agrega una propiedad llamada "ExtraProperties" con un tipo de columna CLOB y un convertidor específico.
    /// Además, se establece un comparador de valores para la propiedad.
    /// </remarks>
    protected override void ApplyExtraProperties( ModelBuilder modelBuilder, IMutableEntityType entityType ) {
        if ( typeof( IExtraProperties ).IsAssignableFrom( entityType.ClrType ) == false )
            return;
        modelBuilder.Entity( entityType.ClrType )
            .Property( "ExtraProperties" )
            .HasColumnName( "ExtraProperties" )
            .HasComment( R.ExtraProperties )
            .HasColumnType( "CLOB" )
            .HasConversion( new ExtraPropertiesValueConverter() )
            .Metadata.SetValueComparer( new ExtraPropertyDictionaryValueComparer() );
    }

    /// <summary>
    /// Configura las convenciones del modelo utilizando el constructor de configuración proporcionado.
    /// </summary>
    /// <param name="configurationBuilder">El constructor de configuración del modelo que se utilizará para establecer las convenciones.</param>
    /// <remarks>
    /// Este método llama a la implementación base para asegurar que las convenciones predeterminadas se apliquen,
    /// y luego se configura una convención específica para convertir GUIDs a cadenas.
    /// </remarks>
    protected override void ConfigureConventions( ModelConfigurationBuilder configurationBuilder ) {
        base.ConfigureConventions( configurationBuilder );
        ConfigGuidToString( configurationBuilder );
    }

    /// <summary>
    /// Configura la conversión de propiedades de tipo Guid a tipo string en el modelo de configuración.
    /// </summary>
    /// <param name="configurationBuilder">El constructor de configuración del modelo que se está configurando.</param>
    /// <remarks>
    /// Este método verifica si la opción de conversión de Guid a string está habilitada en la configuración de Entity Framework.
    /// Si la opción está deshabilitada, el método no realiza ninguna acción.
    /// En caso contrario, se configura la conversión para las propiedades de tipo Guid y Guid? a string.
    /// </remarks>
    protected virtual void ConfigGuidToString( ModelConfigurationBuilder configurationBuilder ) {
        var options = ServiceProvider.GetService<IOptions<OracleEntityFrameworkCoreOptions>>();
        if ( options?.Value.IsGuidToString == false )
            return;
        configurationBuilder.Properties<Guid>().HaveConversion<string>();
        configurationBuilder.Properties<Guid?>().HaveConversion<string>();
    }
}
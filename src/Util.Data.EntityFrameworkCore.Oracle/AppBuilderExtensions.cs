using Util.Configs;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un contexto de trabajo (Unit of Work) para Oracle a la configuración del aplicativo.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa la interfaz <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connection">La cadena de conexión a la base de datos Oracle.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="oracleSetupAction">Una acción opcional para configurar las opciones específicas de Oracle.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar el contexto de trabajo.</param>
    /// <param name="isGuidToString">Un valor booleano que indica si los GUID deben ser convertidos a cadenas.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con el contexto de trabajo agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un contexto de trabajo para realizar operaciones de base de datos utilizando Oracle.
    /// Se pueden especificar acciones de configuración adicionales para el contexto de base de datos y para las opciones específicas de Oracle.
    /// </remarks>
    public static IAppBuilder AddOracleUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<OracleDbContextOptionsBuilder> oracleSetupAction = null, bool? condition = null, bool isGuidToString = true )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddOracleUnitOfWork<TService, TImplementation>( connection, null, setupAction, oracleSetupAction, condition, isGuidToString );
    }

    /// <summary>
    /// Agrega una unidad de trabajo de Oracle al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agrega la unidad de trabajo.</param>
    /// <param name="connection">La conexión a la base de datos de Oracle.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de la base de datos.</param>
    /// <param name="oracleSetupAction">Una acción opcional para configurar las opciones específicas de Oracle.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar la unidad de trabajo.</param>
    /// <param name="isGuidToString">Indica si los GUID deben ser convertidos a cadenas. El valor predeterminado es true.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con la unidad de trabajo de Oracle agregada.</returns>
    /// <remarks>
    /// Este método permite la configuración de una unidad de trabajo específica para Oracle, facilitando la gestión de transacciones y operaciones de base de datos.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddOracleUnitOfWork<TService, TImplementation>( this IAppBuilder builder, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<OracleDbContextOptionsBuilder> oracleSetupAction = null, bool? condition = null, bool isGuidToString = true )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddOracleUnitOfWork<TService, TImplementation>( null, connection, setupAction, oracleSetupAction, condition, isGuidToString );
    }

    /// <summary>
    /// Agrega un contexto de unidad de trabajo de Oracle al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connectionString">La cadena de conexión para la base de datos Oracle.</param>
    /// <param name="connection">La conexión a la base de datos, si se proporciona.</param>
    /// <param name="setupAction">Acción para configurar las opciones del contexto de base de datos.</param>
    /// <param name="oracleSetupAction">Acción para configurar las opciones específicas de Oracle.</param>
    /// <param name="condition">Condición que determina si se debe agregar el contexto de servicio adicional.</param>
    /// <param name="isGuidToString">Indica si los GUID deben ser convertidos a cadenas.</param>
    /// <remarks>
    /// Este método permite configurar un contexto de unidad de trabajo para una base de datos Oracle,
    /// utilizando una cadena de conexión o una conexión existente. También permite personalizar las
    /// opciones del contexto a través de las acciones proporcionadas.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    private static IAppBuilder AddOracleUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connectionString, DbConnection connection, Action<DbContextOptionsBuilder> setupAction,
        Action<OracleDbContextOptionsBuilder> oracleSetupAction, bool? condition, bool isGuidToString )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            void Action( DbContextOptionsBuilder options ) {
                setupAction?.Invoke( options );
                if ( connectionString.IsEmpty() == false ) {
                    options.UseOracle( connectionString, oracleSetupAction );
                    return;
                }
                if ( connection != null ) {
                    options.UseOracle( connection, oracleSetupAction );
                }
            }
            services.AddDbContext<TImplementation>( Action );
            if ( condition == false )
                return;
            services.AddDbContext<TService, TImplementation>( Action );
            ConfigOptions(services, isGuidToString);
        } );
        return builder;
    }

    /// <summary>
    /// Configura las opciones de Entity Framework Core para Oracle.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las opciones.</param>
    /// <param name="isGuidToString">Indica si los GUID deben ser convertidos a cadenas.</param>
    private static void ConfigOptions( IServiceCollection services, bool isGuidToString ) {
        void Action( OracleEntityFrameworkCoreOptions t ) => t.IsGuidToString = isGuidToString;
        services.Configure<OracleEntityFrameworkCoreOptions>( Action );
    }
}
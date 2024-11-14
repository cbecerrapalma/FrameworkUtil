using Util.Configs;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un contexto de trabajo unitario para PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El constructor de aplicaciones donde se registrará el contexto de trabajo.</param>
    /// <param name="connection">La cadena de conexión a la base de datos PostgreSQL.</param>
    /// <param name="setupAction">Acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="pgSqlSetupAction">Acción opcional para configurar las opciones específicas de PostgreSQL.</param>
    /// <param name="isEnableLegacyTimestampBehavior">Indica si se debe habilitar el comportamiento de marca de tiempo legado.</param>
    /// <param name="condition">Condición opcional que determina si se debe agregar el contexto de trabajo.</param>
    /// <returns>El constructor de aplicaciones con el contexto de trabajo agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un contexto de trabajo unitario para interactuar con una base de datos PostgreSQL,
    /// facilitando la gestión de transacciones y operaciones de base de datos.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddPgSqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connection, Action<DbContextOptionsBuilder> setupAction = null,
            Action<NpgsqlDbContextOptionsBuilder> pgSqlSetupAction = null, bool isEnableLegacyTimestampBehavior = false, bool? condition = null )
            where TService : class, IUnitOfWork
            where TImplementation : UnitOfWorkBase, TService {
        return builder.AddPgSqlUnitOfWork<TService, TImplementation>( connection, null, setupAction, pgSqlSetupAction, isEnableLegacyTimestampBehavior, condition );
    }

    /// <summary>
    /// Agrega una unidad de trabajo de PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa la interfaz <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación de la unidad de trabajo, que debe heredar de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El constructor de la aplicación donde se agregará la unidad de trabajo.</param>
    /// <param name="connection">La conexión a la base de datos PostgreSQL.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de la base de datos.</param>
    /// <param name="pgSqlSetupAction">Una acción opcional para configurar las opciones específicas de PostgreSQL.</param>
    /// <param name="isEnableLegacyTimestampBehavior">Indica si se debe habilitar el comportamiento de marca de tiempo legado.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar la unidad de trabajo.</param>
    /// <returns>El constructor de la aplicación con la unidad de trabajo de PostgreSQL agregada.</returns>
    /// <remarks>
    /// Este método permite configurar y agregar una unidad de trabajo que se conecta a una base de datos PostgreSQL,
    /// facilitando la gestión de transacciones y operaciones de base de datos a través de una interfaz común.
    /// </remarks>
    public static IAppBuilder AddPgSqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<NpgsqlDbContextOptionsBuilder> pgSqlSetupAction = null, bool isEnableLegacyTimestampBehavior = false, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddPgSqlUnitOfWork<TService, TImplementation>( null, connection, setupAction, pgSqlSetupAction, isEnableLegacyTimestampBehavior, condition );
    }

    /// <summary>
    /// Agrega un contexto de unidad de trabajo para PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio de unidad de trabajo que se está registrando.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación de la unidad de trabajo que se está registrando.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connectionString">La cadena de conexión a la base de datos PostgreSQL.</param>
    /// <param name="connection">La conexión a la base de datos, si se proporciona.</param>
    /// <param name="setupAction">Acción para configurar las opciones del contexto de la base de datos.</param>
    /// <param name="pgSqlSetupAction">Acción para configurar las opciones específicas de PostgreSQL.</param>
    /// <param name="isEnableLegacyTimestampBehavior">Indica si se debe habilitar el comportamiento de marca de tiempo legado.</param>
    /// <param name="condition">Una condición que determina si se debe registrar el servicio adicional.</param>
    /// <remarks>
    /// Este método permite configurar un contexto de base de datos utilizando Entity Framework Core con soporte para PostgreSQL,
    /// y registra los servicios necesarios en el contenedor de inyección de dependencias.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    private static IAppBuilder AddPgSqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connectionString, DbConnection connection, Action<DbContextOptionsBuilder> setupAction,
        Action<NpgsqlDbContextOptionsBuilder> pgSqlSetupAction, bool isEnableLegacyTimestampBehavior, bool? condition )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        AppContext.SetSwitch( "Npgsql.EnableLegacyTimestampBehavior", isEnableLegacyTimestampBehavior );
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            void Action( DbContextOptionsBuilder options ) {
                setupAction?.Invoke( options );
                if ( connectionString.IsEmpty() == false ) {
                    options.UseNpgsql( connectionString, pgSqlSetupAction );
                    return;
                }
                if ( connection != null ) {
                    options.UseNpgsql( connection, pgSqlSetupAction );
                }
            }
            services.AddDbContext<TImplementation>( Action );
            if ( condition == false )
                return;
            services.AddDbContext<TService, TImplementation>( Action );
        } );
        return builder;
    }
}
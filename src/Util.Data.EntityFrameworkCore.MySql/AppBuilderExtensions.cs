using Util.Configs;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega una unidad de trabajo de MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que implementa la interfaz <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connection">La cadena de conexión a la base de datos MySQL.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones de <see cref="DbContext"/>.</param>
    /// <param name="mySqlSetupAction">Una acción opcional para configurar las opciones específicas de MySQL.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar la unidad de trabajo.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> actualizado.</returns>
    /// <remarks>
    /// Este método permite configurar una unidad de trabajo utilizando MySQL, facilitando la gestión de transacciones y operaciones de base de datos.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddMySqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<MySqlDbContextOptionsBuilder> mySqlSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddMySqlUnitOfWork<TService, TImplementation>( connection, null, setupAction, mySqlSetupAction, condition );
    }

    /// <summary>
    /// Agrega un contexto de trabajo (Unit of Work) para MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> en el que se agregará el contexto de trabajo.</param>
    /// <param name="connection">La conexión a la base de datos MySQL.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="mySqlSetupAction">Una acción opcional para configurar las opciones específicas de MySQL.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar el contexto de trabajo.</param>
    /// <returns>El <see cref="IAppBuilder"/> con el contexto de trabajo agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un contexto de trabajo para interactuar con una base de datos MySQL,
    /// facilitando la gestión de transacciones y operaciones de base de datos.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddMySqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<MySqlDbContextOptionsBuilder> mySqlSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddMySqlUnitOfWork<TService, TImplementation>( null, connection, setupAction, mySqlSetupAction, condition );
    }

    /// <summary>
    /// Agrega un contexto de unidad de trabajo de MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa la interfaz <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connectionString">La cadena de conexión para la base de datos MySQL.</param>
    /// <param name="connection">Una conexión a la base de datos MySQL.</param>
    /// <param name="setupAction">Una acción para configurar las opciones del contexto de base de datos.</param>
    /// <param name="mySqlSetupAction">Una acción para configurar las opciones específicas de MySQL.</param>
    /// <param name="condition">Una condición que determina si se debe agregar el servicio adicional.</param>
    /// <remarks>
    /// Este método permite configurar un contexto de base de datos MySQL utilizando una cadena de conexión o una conexión existente.
    /// Si se proporciona una cadena de conexión, se utilizará para configurar el contexto; de lo contrario, se utilizará la conexión proporcionada.
    /// </remarks>
    /// <returns>El objeto <see cref="IAppBuilder"/> para permitir la configuración en cadena.</returns>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    private static IAppBuilder AddMySqlUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connectionString, DbConnection connection, Action<DbContextOptionsBuilder> setupAction,
        Action<MySqlDbContextOptionsBuilder> mySqlSetupAction, bool? condition )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            void Action( DbContextOptionsBuilder options ) {
                setupAction?.Invoke( options );
                if ( connectionString.IsEmpty() == false ) {
                    options.UseMySql( connectionString, ServerVersion.AutoDetect( connectionString ),
                        optionsBuilder => {
                            optionsBuilder.EnableRetryOnFailure();
                            mySqlSetupAction?.Invoke( optionsBuilder );
                        } );
                    return;
                }
                if ( connection != null ) {
                    options.UseMySql( connection, ServerVersion.AutoDetect( (MySqlConnection)connection ), optionsBuilder => {
                        optionsBuilder.EnableRetryOnFailure();
                        mySqlSetupAction?.Invoke( optionsBuilder );
                    } );
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
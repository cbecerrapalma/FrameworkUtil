using Util.Configs;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un contexto de trabajo de SQLite a la configuración del aplicativo.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de la implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connection">La cadena de conexión para la base de datos SQLite.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="sqliteSetupAction">Una acción opcional para configurar las opciones específicas de SQLite.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar el contexto de trabajo.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> actualizado con el contexto de trabajo de SQLite agregado.</returns>
    /// <remarks>
    /// Este método permite la configuración de un contexto de trabajo utilizando SQLite como base de datos.
    /// Se pueden proporcionar acciones de configuración adicionales para personalizar el comportamiento del contexto.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddSqliteUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<SqliteDbContextOptionsBuilder> sqliteSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddSqliteUnitOfWork<TService, TImplementation>( connection, null, setupAction, sqliteSetupAction, condition );
    }

    /// <summary>
    /// Agrega un contexto de trabajo (Unit of Work) utilizando SQLite a la configuración del contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El constructor de la aplicación donde se agregará el contexto de trabajo.</param>
    /// <param name="connection">La conexión a la base de datos SQLite.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de la base de datos.</param>
    /// <param name="sqliteSetupAction">Una acción opcional para configurar las opciones específicas de SQLite.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar el contexto de trabajo.</param>
    /// <returns>El mismo constructor de la aplicación con el contexto de trabajo agregado.</returns>
    /// <remarks>
    /// Este método permite la configuración de un contexto de trabajo utilizando una conexión a una base de datos SQLite.
    /// Se pueden proporcionar acciones opcionales para personalizar la configuración del contexto y las opciones específicas de SQLite.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddSqliteUnitOfWork<TService, TImplementation>( this IAppBuilder builder, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<SqliteDbContextOptionsBuilder> sqliteSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddSqliteUnitOfWork<TService, TImplementation>( null, connection, setupAction, sqliteSetupAction, condition );
    }

    /// <summary>
    /// Agrega un contexto de unidad de trabajo utilizando SQLite al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que representa la unidad de trabajo.</typeparam>
    /// <typeparam name="TImplementation">La implementación concreta de la unidad de trabajo.</typeparam>
    /// <param name="builder">El constructor de aplicaciones que se está configurando.</param>
    /// <param name="connectionString">La cadena de conexión para la base de datos SQLite.</param>
    /// <param name="connection">La conexión a la base de datos SQLite.</param>
    /// <param name="setupAction">Acción para configurar las opciones del contexto de base de datos.</param>
    /// <param name="sqliteSetupAction">Acción para configurar las opciones específicas de SQLite.</param>
    /// <param name="condition">Condición que determina si se debe agregar el servicio adicional.</param>
    /// <remarks>
    /// Este método permite configurar un contexto de unidad de trabajo que utiliza SQLite como base de datos.
    /// Se puede proporcionar una cadena de conexión o una conexión existente. Además, se pueden aplicar configuraciones adicionales
    /// a través de las acciones proporcionadas.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    private static IAppBuilder AddSqliteUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connectionString, DbConnection connection, Action<DbContextOptionsBuilder> setupAction,
        Action<SqliteDbContextOptionsBuilder> sqliteSetupAction, bool? condition )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            void Action( DbContextOptionsBuilder options ) {
                setupAction?.Invoke( options );
                if ( connectionString.IsEmpty() == false ) {
                    options.UseSqlite( connectionString, sqliteSetupAction );
                    return;
                }
                if ( connection != null ) {
                    options.UseSqlite( connection, sqliteSetupAction );
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
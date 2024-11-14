using Util.Configs;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para la configuración de aplicaciones.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega una unidad de trabajo basada en SQL Server al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connection">La cadena de conexión a la base de datos SQL Server.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de la base de datos.</param>
    /// <param name="sqlServerSetupAction">Una acción opcional para configurar las opciones específicas de SQL Server.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar la unidad de trabajo.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado.</returns>
    /// <remarks>
    /// Este método permite registrar una unidad de trabajo en el contenedor de servicios, facilitando la gestión de transacciones y el acceso a la base de datos.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    public static IAppBuilder AddSqlServerUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<SqlServerDbContextOptionsBuilder> sqlServerSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddSqlServerUnitOfWork<TService, TImplementation>( connection, null, setupAction, sqlServerSetupAction, condition );
    }

    /// <summary>
    /// Agrega una unidad de trabajo basada en SQL Server al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa la interfaz <see cref="IUnitOfWork"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="UnitOfWorkBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El constructor de aplicaciones que se está configurando.</param>
    /// <param name="connection">La conexión a la base de datos SQL Server.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="sqlServerSetupAction">Una acción opcional para configurar las opciones específicas de SQL Server.</param>
    /// <param name="condition">Una condición opcional que determina si se debe agregar la unidad de trabajo.</param>
    /// <returns>El constructor de aplicaciones configurado.</returns>
    /// <remarks>
    /// Este método permite registrar una unidad de trabajo que utiliza SQL Server como su proveedor de base de datos.
    /// Se puede personalizar la configuración del contexto de base de datos a través de los parámetros opcionales.
    /// </remarks>
    public static IAppBuilder AddSqlServerUnitOfWork<TService, TImplementation>( this IAppBuilder builder, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<SqlServerDbContextOptionsBuilder> sqlServerSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        return builder.AddSqlServerUnitOfWork<TService, TImplementation>( null, connection, setupAction, sqlServerSetupAction, condition );
    }

    /// <summary>
    /// Agrega un contexto de unidad de trabajo basado en SQL Server al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio de unidad de trabajo que se va a registrar.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación de la unidad de trabajo que se va a registrar.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connectionString">La cadena de conexión para la base de datos SQL Server.</param>
    /// <param name="connection">Una conexión a la base de datos SQL Server.</param>
    /// <param name="setupAction">Una acción opcional para configurar las opciones del contexto de base de datos.</param>
    /// <param name="sqlServerSetupAction">Una acción opcional para configurar las opciones específicas de SQL Server.</param>
    /// <param name="condition">Una condición opcional que determina si se debe registrar el servicio.</param>
    /// <remarks>
    /// Este método permite configurar un contexto de base de datos utilizando SQL Server y registrar
    /// tanto el servicio de unidad de trabajo como su implementación en el contenedor de servicios.
    /// Si se proporciona una cadena de conexión, se utilizará en lugar de la conexión.
    /// </remarks>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkBase"/>
    private static IAppBuilder AddSqlServerUnitOfWork<TService, TImplementation>( this IAppBuilder builder, string connectionString, DbConnection connection, Action<DbContextOptionsBuilder> setupAction = null,
        Action<SqlServerDbContextOptionsBuilder> sqlServerSetupAction = null, bool? condition = null )
        where TService : class, IUnitOfWork
        where TImplementation : UnitOfWorkBase, TService {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            void Action( DbContextOptionsBuilder options ) {
                setupAction?.Invoke( options );
                if ( connectionString.IsEmpty() == false ) {
                    options.UseSqlServer( connectionString, sqlServerSetupAction );
                    return;
                }
                if ( connection != null ) {
                    options.UseSqlServer( connection, sqlServerSetupAction );
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
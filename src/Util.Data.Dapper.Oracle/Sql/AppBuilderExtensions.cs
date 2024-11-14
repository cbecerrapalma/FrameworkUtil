using Util.Configs;
using Util.Data.Dapper.TypeHandlers;
using Util.Data.Sql;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Proporciona métodos de extensión para construir y configurar aplicaciones.
/// </summary>
public static class AppBuilderExtensions
{

    #region AddOracleSqlQuery(Configurar el objeto de consulta de Oracle SQL.)

    /// <summary>
    /// Agrega una consulta de Oracle SQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agrega la consulta.</param>
    /// <returns>El mismo constructor de aplicaciones con la consulta de Oracle SQL añadida.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar una consulta de Oracle SQL sin parámetros.
    /// </remarks>
    public static IAppBuilder AddOracleSqlQuery(this IAppBuilder builder)
    {
        return builder.AddOracleSqlQuery("");
    }

    /// <summary>
    /// Agrega una consulta de Oracle SQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agrega la consulta.</param>
    /// <param name="connection">La cadena de conexión para la base de datos Oracle.</param>
    /// <returns>El constructor de la aplicación con la consulta de Oracle SQL añadida.</returns>
    public static IAppBuilder AddOracleSqlQuery(this IAppBuilder builder, string connection)
    {
        return builder.AddOracleSqlQuery<ISqlQuery, OracleSqlQuery>(connection);
    }

    /// <summary>
    /// Agrega una consulta SQL de Oracle al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará la consulta SQL.</param>
    /// <param name="setupAction">Acción que configura las opciones de SQL.</param>
    /// <returns>El constructor de la aplicación con la consulta SQL de Oracle agregada.</returns>
    public static IAppBuilder AddOracleSqlQuery(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddOracleSqlQuery<ISqlQuery, OracleSqlQuery>(setupAction);
    }

    /// <summary>
    /// Agrega una implementación de consulta SQL de Oracle al contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="OracleSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> donde se agrega la implementación.</param>
    /// <param name="connection">La cadena de conexión a la base de datos Oracle.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado con la implementación de consulta SQL de Oracle.</returns>
    /// <remarks>
    /// Este método permite registrar una consulta SQL de Oracle en el contenedor de aplicaciones, 
    /// utilizando la cadena de conexión proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="OracleSqlQueryBase"/>
    public static IAppBuilder AddOracleSqlQuery<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlQuery
        where TImplementation : OracleSqlQueryBase, TService
    {
        return builder.AddOracleSqlQuery<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega una implementación de consulta SQL de Oracle al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="OracleSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> en el que se registrará la implementación.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El mismo <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite registrar una implementación específica de consulta SQL de Oracle
    /// y configurar sus opciones antes de que se registre en el contenedor de servicios.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="OracleSqlQueryBase"/>
    /// <seealso cref="SqlOptions{T}"/>
    public static IAppBuilder AddOracleSqlQuery<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlQuery
        where TImplementation : OracleSqlQueryBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
            RegisterGuidTypeHandler();
        });
        return builder;
    }

    /// <summary>
    /// Registra un manejador de tipo personalizado para el tipo <see cref="Guid"/> en Dapper.
    /// </summary>
    /// <remarks>
    /// Este método elimina cualquier mapeo existente para el tipo <see cref="Guid"/> y su versión nullable <see cref="Guid?"/>.
    /// Luego, añade un nuevo manejador de tipo que permite a Dapper manejar correctamente los valores de tipo <see cref="Guid"/>.
    /// </remarks>
    private static void RegisterGuidTypeHandler()
    {
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));
        SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
    }

    #endregion

    #region AddOracleSqlExecutor(Configurar el ejecutor de Oracle SQL.)

    /// <summary>
    /// Agrega un ejecutor de SQL de Oracle al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agrega el ejecutor de SQL de Oracle.</param>
    /// <returns>El constructor de la aplicación con el ejecutor de SQL de Oracle agregado.</returns>
    public static IAppBuilder AddOracleSqlExecutor(this IAppBuilder builder)
    {
        return builder.AddOracleSqlExecutor("");
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de Oracle al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se añadirá el ejecutor.</param>
    /// <param name="connection">La cadena de conexión para la base de datos de Oracle.</param>
    /// <returns>El constructor de la aplicación con el ejecutor de SQL de Oracle agregado.</returns>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="OracleSqlExecutor"/>
    public static IAppBuilder AddOracleSqlExecutor(this IAppBuilder builder, string connection)
    {
        return builder.AddOracleSqlExecutor<ISqlExecutor, OracleSqlExecutor>(connection);
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de Oracle al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agrega el ejecutor de SQL.</param>
    /// <param name="setupAction">Acción para configurar las opciones de SQL.</param>
    /// <returns>El constructor de aplicaciones con el ejecutor de SQL agregado.</returns>
    public static IAppBuilder AddOracleSqlExecutor(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddOracleSqlExecutor<ISqlExecutor, OracleSqlExecutor>(setupAction);
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de Oracle al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="OracleSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> donde se agrega el ejecutor.</param>
    /// <param name="connection">La cadena de conexión para la base de datos de Oracle.</param>
    /// <returns>La instancia actual de <see cref="IAppBuilder"/> con el ejecutor de SQL de Oracle agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un ejecutor de SQL de Oracle con una cadena de conexión específica.
    /// Asegúrese de que el tipo de implementación proporcionado cumpla con los requisitos de tipo.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="OracleSqlExecutorBase"/>
    public static IAppBuilder AddOracleSqlExecutor<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlExecutor
        where TImplementation : OracleSqlExecutorBase, TService
    {
        return builder.AddOracleSqlExecutor<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de Oracle al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de la implementación que hereda de <see cref="OracleSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado para incluir el ejecutor de SQL de Oracle.</returns>
    /// <remarks>
    /// Este método permite registrar un ejecutor de SQL de Oracle en el contenedor de servicios de la aplicación,
    /// configurando las opciones necesarias a través de la acción proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="OracleSqlExecutorBase"/>
    public static IAppBuilder AddOracleSqlExecutor<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlExecutor
        where TImplementation : OracleSqlExecutorBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.Host.ConfigureServices((context, services) =>
        {
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
            RegisterGuidTypeHandler();
        });
        return builder;
    }

    #endregion
}
using Util.Configs;
using Util.Data.Sql;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions
{

    #region AddSqlServerSqlQuery(Configurar el objeto de consulta SQL de Sql Server.)

    /// <summary>
    /// Agrega soporte para consultas SQL Server al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <returns>
    /// El objeto <see cref="IAppBuilder"/> modificado con el soporte para consultas SQL Server.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que utiliza una cadena vacía como parámetro.
    /// </remarks>
    public static IAppBuilder AddSqlServerSqlQuery(this IAppBuilder builder)
    {
        return builder.AddSqlServerSqlQuery("");
    }

    /// <summary>
    /// Agrega un servicio de consulta SQL Server al contenedor de aplicaciones.
    /// </summary>
    /// <param name="builder">El contenedor de aplicaciones donde se agregará el servicio.</param>
    /// <param name="connection">La cadena de conexión para conectarse a la base de datos SQL Server.</param>
    /// <returns>El contenedor de aplicaciones actualizado con el servicio de consulta SQL Server agregado.</returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar la consulta SQL Server 
    /// de manera sencilla, utilizando el tipo de consulta genérico <see cref="ISqlQuery"/> 
    /// y su implementación <see cref="SqlServerSqlQuery"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="SqlServerSqlQuery"/>
    public static IAppBuilder AddSqlServerSqlQuery(this IAppBuilder builder, string connection)
    {
        return builder.AddSqlServerSqlQuery<ISqlQuery, SqlServerSqlQuery>(connection);
    }

    /// <summary>
    /// Agrega la funcionalidad de consulta SQL Server al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agregará la funcionalidad.</param>
    /// <param name="setupAction">Acción para configurar las opciones de SQL.</param>
    /// <returns>El constructor de aplicaciones modificado.</returns>
    public static IAppBuilder AddSqlServerSqlQuery(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddSqlServerSqlQuery<ISqlQuery, SqlServerSqlQuery>(setupAction);
    }

    /// <summary>
    /// Agrega una implementación de consulta SQL Server al contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de la implementación que hereda de <see cref="SqlServerSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agrega la consulta SQL Server.</param>
    /// <param name="connection">La cadena de conexión a la base de datos SQL Server.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para incluir la consulta SQL Server.</returns>
    /// <remarks>
    /// Este método permite configurar la conexión a la base de datos SQL Server para el servicio especificado.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="SqlServerSqlQueryBase"/>
    public static IAppBuilder AddSqlServerSqlQuery<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlQuery
        where TImplementation : SqlServerSqlQueryBase, TService
    {
        return builder.AddSqlServerSqlQuery<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega una implementación de consulta SQL Server al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de la implementación que hereda de <see cref="SqlServerSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado.</returns>
    /// <remarks>
    /// Este método permite registrar una implementación específica de consulta SQL Server en el contenedor de servicios,
    /// así como configurar las opciones necesarias para su funcionamiento.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="SqlServerSqlQueryBase"/>
    /// <seealso cref="SqlOptions{T}"/>
    public static IAppBuilder AddSqlServerSqlQuery<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlQuery
        where TImplementation : SqlServerSqlQueryBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
        });
        return builder;
    }

    #endregion

    #region AddSqlServerSqlExecutor(Configurar el ejecutor SQL de Sql Server.)

    /// <summary>
    /// Agrega un ejecutor de SQL Server a la configuración del constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agregará el ejecutor de SQL Server.</param>
    /// <returns>El mismo constructor de aplicaciones con el ejecutor de SQL Server agregado.</returns>
    public static IAppBuilder AddSqlServerSqlExecutor(this IAppBuilder builder)
    {
        return builder.AddSqlServerSqlExecutor("");
    }

    /// <summary>
    /// Agrega un ejecutor SQL de SQL Server al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá el ejecutor SQL.</param>
    /// <param name="connection">La cadena de conexión para la base de datos SQL Server.</param>
    /// <returns>El constructor de aplicaciones con el ejecutor SQL agregado.</returns>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="SqlServerSqlExecutor"/>
    public static IAppBuilder AddSqlServerSqlExecutor(this IAppBuilder builder, string connection)
    {
        return builder.AddSqlServerSqlExecutor<ISqlExecutor, SqlServerSqlExecutor>(connection);
    }

    /// <summary>
    /// Agrega un ejecutor SQL de SQL Server al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agregará el ejecutor SQL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El constructor de la aplicación modificado.</returns>
    public static IAppBuilder AddSqlServerSqlExecutor(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddSqlServerSqlExecutor<ISqlExecutor, SqlServerSqlExecutor>(setupAction);
    }

    /// <summary>
    /// Agrega un ejecutor SQL de SQL Server al contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de la implementación que hereda de <see cref="SqlServerSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> al que se le agregará el ejecutor SQL.</param>
    /// <param name="connection">La cadena de conexión para la base de datos SQL Server.</param>
    /// <returns>El mismo <see cref="IAppBuilder"/> con el ejecutor SQL agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un ejecutor SQL para interactuar con una base de datos SQL Server,
    /// utilizando la cadena de conexión proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="SqlServerSqlExecutorBase"/>
    public static IAppBuilder AddSqlServerSqlExecutor<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlExecutor
        where TImplementation : SqlServerSqlExecutorBase, TService
    {
        return builder.AddSqlServerSqlExecutor<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Extensión para agregar un ejecutor SQL de SQL Server al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="SqlServerSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Acción que permite configurar las opciones de SQL.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para permitir la configuración adicional.</returns>
    /// <remarks>
    /// Este método permite registrar un ejecutor SQL de SQL Server en el contenedor de servicios,
    /// utilizando una acción de configuración para establecer las opciones necesarias.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="SqlServerSqlExecutorBase"/>
    /// <seealso cref="SqlOptions{TImplementation}"/>
    public static IAppBuilder AddSqlServerSqlExecutor<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlExecutor
        where TImplementation : SqlServerSqlExecutorBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.Host.ConfigureServices((context, services) =>
        {
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
        });
        return builder;
    }

    #endregion
}
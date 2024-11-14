using Util.Configs;
using Util.Data.Sql;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions
{

    #region AddPgSqlQuery(Configurar el objeto de consulta SQL de PostgreSQL.)

    /// <summary>
    /// Agrega una consulta de PostgreSQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se añadirá la consulta de PostgreSQL.</param>
    /// <returns>El constructor de la aplicación con la consulta de PostgreSQL añadida.</returns>
    public static IAppBuilder AddPgSqlQuery(this IAppBuilder builder)
    {
        return builder.AddPgSqlQuery("");
    }

    /// <summary>
    /// Agrega un servicio de consulta PostgreSQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el servicio.</param>
    /// <param name="connection">La cadena de conexión para la base de datos PostgreSQL.</param>
    /// <returns>El constructor de la aplicación con el servicio de consulta agregado.</returns>
    public static IAppBuilder AddPgSqlQuery(this IAppBuilder builder, string connection)
    {
        return builder.AddPgSqlQuery<ISqlQuery, PostgreSqlQuery>(connection);
    }

    /// <summary>
    /// Agrega la funcionalidad de consulta de PostgreSQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agregarán las consultas de PostgreSQL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El constructor de la aplicación con la funcionalidad de consulta de PostgreSQL añadida.</returns>
    public static IAppBuilder AddPgSqlQuery(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddPgSqlQuery<ISqlQuery, PostgreSqlQuery>(setupAction);
    }

    /// <summary>
    /// Agrega una implementación de consulta SQL de PostgreSQL al contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="PostgreSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> al que se le agregará la consulta SQL.</param>
    /// <param name="connection">La cadena de conexión a la base de datos PostgreSQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado con la consulta SQL agregada.</returns>
    /// <remarks>
    /// Este método permite registrar una implementación específica de consulta SQL de PostgreSQL 
    /// en el contenedor de aplicaciones, utilizando la cadena de conexión proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="PostgreSqlQueryBase"/>
    public static IAppBuilder AddPgSqlQuery<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlQuery
        where TImplementation : PostgreSqlQueryBase, TService
    {
        return builder.AddPgSqlQuery<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega una consulta de PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="PostgreSqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> en el que se agregará la consulta.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado para permitir la configuración de consultas de PostgreSQL.</returns>
    /// <remarks>
    /// Este método permite registrar una implementación específica de una consulta SQL en el contenedor de servicios,
    /// así como configurar opciones adicionales a través de la acción proporcionada.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="PostgreSqlQueryBase"/>
    /// <seealso cref="SqlOptions{T}"/>
    public static IAppBuilder AddPgSqlQuery<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlQuery
        where TImplementation : PostgreSqlQueryBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
        });
        return builder;
    }

    #endregion

    #region AddPgSqlExecutor(Configurar el ejecutor SQL de PostgreSQL.)

    /// <summary>
    /// Agrega un ejecutor de PostgreSQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agregará el ejecutor.</param>
    /// <returns>El constructor de la aplicación con el ejecutor de PostgreSQL agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar un ejecutor de PostgreSQL sin especificar un parámetro adicional.
    /// </remarks>
    public static IAppBuilder AddPgSqlExecutor(this IAppBuilder builder)
    {
        return builder.AddPgSqlExecutor("");
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de PostgreSQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agregará el ejecutor.</param>
    /// <param name="connection">La cadena de conexión para la base de datos PostgreSQL.</param>
    /// <returns>El constructor de aplicaciones con el ejecutor de SQL agregado.</returns>
    public static IAppBuilder AddPgSqlExecutor(this IAppBuilder builder, string connection)
    {
        return builder.AddPgSqlExecutor<ISqlExecutor, PostgreSqlExecutor>(connection);
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de PostgreSQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá el ejecutor de SQL.</param>
    /// <param name="setupAction">Acción para configurar las opciones de SQL.</param>
    /// <returns>El constructor de aplicaciones actualizado con el ejecutor de SQL de PostgreSQL.</returns>
    public static IAppBuilder AddPgSqlExecutor(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddPgSqlExecutor<ISqlExecutor, PostgreSqlExecutor>(setupAction);
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="PostgreSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agrega el ejecutor.</param>
    /// <param name="connection">La cadena de conexión para la base de datos PostgreSQL.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> actualizado con el ejecutor agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un ejecutor de SQL para interactuar con una base de datos PostgreSQL
    /// utilizando la cadena de conexión proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="PostgreSqlExecutorBase"/>
    public static IAppBuilder AddPgSqlExecutor<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlExecutor
        where TImplementation : PostgreSqlExecutorBase, TService
    {
        return builder.AddPgSqlExecutor<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega un ejecutor de SQL de PostgreSQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="PostgreSqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> al que se le agrega el ejecutor de SQL.</param>
    /// <param name="setupAction">Una acción para configurar las opciones de SQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado con el ejecutor de SQL agregado.</returns>
    /// <remarks>
    /// Este método permite configurar un ejecutor de SQL para PostgreSQL, 
    /// permitiendo la inyección de dependencias y la configuración de opciones específicas.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="PostgreSqlExecutorBase"/>
    public static IAppBuilder AddPgSqlExecutor<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlExecutor
        where TImplementation : PostgreSqlExecutorBase, TService
    {
        var options = new SqlOptions<TImplementation>();
        setupAction?.Invoke(options);
        builder.Host.ConfigureServices((context, services) =>
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            services.TryAddTransient(typeof(TService), typeof(TImplementation));
            services.TryAddSingleton(typeof(SqlOptions<TImplementation>), (sp) => options);
        });
        return builder;
    }

    #endregion
}
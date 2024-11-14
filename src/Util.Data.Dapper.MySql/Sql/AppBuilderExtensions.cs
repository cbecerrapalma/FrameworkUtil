using Util.Configs;
using Util.Data.Sql;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions
{

    #region AddMySqlQuery(Configurar el objeto de consulta SQL de MySql.)

    /// <summary>
    /// Agrega una consulta MySQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá la consulta MySQL.</param>
    /// <returns>El mismo constructor de aplicaciones con la consulta MySQL añadida.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite añadir una consulta MySQL sin especificar un argumento.
    /// </remarks>
    /// <seealso cref="AddMySqlQuery(IAppBuilder, string)"/>
    public static IAppBuilder AddMySqlQuery(this IAppBuilder builder)
    {
        return builder.AddMySqlQuery("");
    }

    /// <summary>
    /// Agrega una consulta MySQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agrega la consulta.</param>
    /// <param name="connection">La cadena de conexión para la base de datos MySQL.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado con la consulta MySQL añadida.</returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar una consulta MySQL en el contexto de la aplicación.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="MySqlQuery"/>
    public static IAppBuilder AddMySqlQuery(this IAppBuilder builder, string connection)
    {
        return builder.AddMySqlQuery<ISqlQuery, MySqlQuery>(connection);
    }

    /// <summary>
    /// Agrega una consulta de MySQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>
    /// El objeto <see cref="IAppBuilder"/> modificado con la consulta de MySQL agregada.
    /// </returns>
    /// <seealso cref="SqlOptions"/>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="MySqlQuery"/>
    public static IAppBuilder AddMySqlQuery(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddMySqlQuery<ISqlQuery, MySqlQuery>(setupAction);
    }

    /// <summary>
    /// Agrega una implementación de consulta MySQL al contenedor de aplicaciones.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="MySqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> en el que se agrega la consulta MySQL.</param>
    /// <param name="connection">La cadena de conexión a la base de datos MySQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado con la consulta MySQL agregada.</returns>
    /// <remarks>
    /// Este método permite configurar una consulta MySQL personalizada en el contenedor de aplicaciones,
    /// utilizando una cadena de conexión proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="MySqlQueryBase"/>
    public static IAppBuilder AddMySqlQuery<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlQuery
        where TImplementation : MySqlQueryBase, TService
    {
        return builder.AddMySqlQuery<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega una implementación de consulta MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo de servicio que implementa <see cref="ISqlQuery"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="MySqlQueryBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> en el que se agregará la consulta MySQL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado para incluir la implementación de consulta MySQL.</returns>
    /// <remarks>
    /// Este método permite registrar una implementación de consulta MySQL en el contenedor de servicios,
    /// proporcionando una forma de configurar las opciones necesarias a través de la acción <paramref name="setupAction"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="MySqlQueryBase"/>
    public static IAppBuilder AddMySqlQuery<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlQuery
        where TImplementation : MySqlQueryBase, TService
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

    #region AddMySqlExecutor(Configurar el ejecutor SQL de MySql.)

    /// <summary>
    /// Agrega un ejecutor de MySQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agregará el ejecutor de MySQL.</param>
    /// <returns>El constructor de aplicaciones actualizado con el ejecutor de MySQL agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que utiliza una cadena vacía como parámetro adicional.
    /// </remarks>
    public static IAppBuilder AddMySqlExecutor(this IAppBuilder builder)
    {
        return builder.AddMySqlExecutor("");
    }

    /// <summary>
    /// Agrega un ejecutor de MySQL al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá el ejecutor de MySQL.</param>
    /// <param name="connection">La cadena de conexión para la base de datos MySQL.</param>
    /// <returns>El mismo constructor de aplicaciones con el ejecutor de MySQL agregado.</returns>
    public static IAppBuilder AddMySqlExecutor(this IAppBuilder builder, string connection)
    {
        return builder.AddMySqlExecutor<ISqlExecutor, MySqlExecutor>(connection);
    }

    /// <summary>
    /// Agrega un ejecutor de MySQL al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el ejecutor de MySQL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de SQL.</param>
    /// <returns>El constructor de la aplicación con el ejecutor de MySQL agregado.</returns>
    public static IAppBuilder AddMySqlExecutor(this IAppBuilder builder, Action<SqlOptions> setupAction)
    {
        return builder.AddMySqlExecutor<ISqlExecutor, MySqlExecutor>(setupAction);
    }

    /// <summary>
    /// Agrega un ejecutor de MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="MySqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="connection">La cadena de conexión para la base de datos MySQL.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado.</returns>
    /// <remarks>
    /// Este método permite registrar un ejecutor de MySQL en el contenedor de servicios, 
    /// configurando la cadena de conexión necesaria para la conexión a la base de datos.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="MySqlExecutorBase"/>
    public static IAppBuilder AddMySqlExecutor<TService, TImplementation>(this IAppBuilder builder, string connection)
        where TService : ISqlExecutor
        where TImplementation : MySqlExecutorBase, TService
    {
        return builder.AddMySqlExecutor<TService, TImplementation>(t => t.ConnectionString(connection));
    }

    /// <summary>
    /// Agrega un ejecutor de MySQL al contenedor de servicios.
    /// </summary>
    /// <typeparam name="TService">El tipo del servicio que implementa <see cref="ISqlExecutor"/>.</typeparam>
    /// <typeparam name="TImplementation">El tipo de implementación que hereda de <see cref="MySqlExecutorBase"/> y de <typeparamref name="TService"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> donde se registrará el ejecutor de MySQL.</param>
    /// <param name="setupAction">Una acción para configurar las opciones del ejecutor SQL.</param>
    /// <returns>El mismo <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite registrar un ejecutor de MySQL en el contenedor de servicios de la aplicación, 
    /// configurando las opciones necesarias a través de la acción proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlExecutor"/>
    /// <seealso cref="MySqlExecutorBase"/>
    public static IAppBuilder AddMySqlExecutor<TService, TImplementation>(this IAppBuilder builder, Action<SqlOptions> setupAction)
        where TService : ISqlExecutor
        where TImplementation : MySqlExecutorBase, TService
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
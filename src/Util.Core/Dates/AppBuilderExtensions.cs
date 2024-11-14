using Util.Configs;

namespace Util.Dates;

/// <summary>
/// Proporciona métodos de extensión para configurar la aplicación.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega soporte para la zona horaria UTC al <see cref="IAppBuilder"/>.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> a la que se le agrega el soporte UTC.</param>
    /// <returns>La misma instancia de <see cref="IAppBuilder"/> con el soporte UTC agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que llama a <see cref="AddUtc(bool)"/> con el valor predeterminado de <c>true</c>.
    /// </remarks>
    /// <seealso cref="AddUtc(bool)"/>
    public static IAppBuilder AddUtc( this IAppBuilder builder ) {
        return builder.AddUtc( true );
    }

    /// <summary>
    /// Agrega la configuración de uso de UTC al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación que se está configurando.</param>
    /// <param name="isUseUtc">Un valor booleano que indica si se debe utilizar UTC.</param>
    /// <returns>El mismo constructor de la aplicación con la configuración actualizada.</returns>
    /// <remarks>
    /// Este método permite establecer si la aplicación debe operar en horario UTC.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="builder"/> es nulo.</exception>
    public static IAppBuilder AddUtc( this IAppBuilder builder, bool isUseUtc ) {
        builder.CheckNull( nameof( builder ) );
        TimeOptions.IsUseUtc = isUseUtc;
        return builder;
    }
}
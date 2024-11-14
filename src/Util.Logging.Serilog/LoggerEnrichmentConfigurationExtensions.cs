using Util.Logging.Serilog.Enrichers;

namespace Util.Logging.Serilog; 

/// <summary>
/// Proporciona métodos de extensión para la configuración de enriquecimiento de registros.
/// </summary>
public static class LoggerEnrichmentConfigurationExtensions {
    /// <summary>
    /// Extiende la configuración de enriquecimiento de registros para incluir un contexto de registro.
    /// </summary>
    /// <param name="source">La configuración de enriquecimiento de registros que se está extendiendo.</param>
    /// <returns>La configuración de registros enriquecida con el contexto de registro.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Esta extensión permite agregar un <see cref="LogContextEnricher"/> a la configuración de registros,
    /// lo que facilita la inclusión de información contextual en los registros generados.
    /// </remarks>
    /// <seealso cref="LogContextEnricher"/>
    public static LoggerConfiguration WithLogContext( this LoggerEnrichmentConfiguration source ) {
        source.CheckNull( nameof( source ) );
        return source.With<LogContextEnricher>();
    }

    /// <summary>
    /// Extiende la configuración de enriquecimiento del registrador para incluir un nivel de registro.
    /// </summary>
    /// <param name="source">La configuración de enriquecimiento del registrador a la que se le aplicará el nivel de registro.</param>
    /// <returns>Una nueva configuración de registrador con el enriquecedor de nivel de registro aplicado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static LoggerConfiguration WithLogLevel( this LoggerEnrichmentConfiguration source ) {
        source.CheckNull( nameof( source ) );
        return source.With<LogLevelEnricher>();
    }
}
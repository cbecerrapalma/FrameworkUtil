namespace Util.ObjectMapping; 

/// <summary>
/// Define la configuración de mapeo para AutoMapper.
/// </summary>
public interface IAutoMapperConfig {
    /// <summary>
    /// Configura la expresión de mapeo para el perfil de AutoMapper.
    /// </summary>
    /// <param name="expression">La expresión de configuración de mapeo que se va a modificar.</param>
    /// <remarks>
    /// Este método permite definir cómo se deben mapear los objetos de un tipo a otro,
    /// configurando las reglas necesarias para el mapeo de propiedades y tipos.
    /// </remarks>
    void Config(IMapperConfigurationExpression expression);
}
using Util.Ui.Configs;

namespace Util.Ui.Expressions; 

/// <summary>
/// Define un contrato para cargar expresiones.
/// </summary>
public interface IExpressionLoader {
    /// <summary>
    /// Carga la configuración especificada y establece la propiedad de expresión.
    /// </summary>
    /// <param name="config">La configuración que se va a cargar.</param>
    /// <param name="expressionPropertyName">El nombre de la propiedad de expresión que se va a utilizar. Por defecto es "for".</param>
    /// <remarks>
    /// Este método permite inicializar la configuración de un objeto utilizando los parámetros proporcionados.
    /// Asegúrese de que la configuración sea válida antes de llamar a este método.
    /// </remarks>
    void Load(Config config, string expressionPropertyName = "for");
}
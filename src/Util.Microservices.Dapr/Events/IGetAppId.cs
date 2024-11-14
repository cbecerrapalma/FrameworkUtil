namespace Util.Microservices.Dapr.Events; 

/// <summary>
/// Interfaz que define un contrato para obtener el identificador de la aplicación.
/// Esta interfaz hereda de <see cref="ISingletonDependency"/> lo que indica que debe ser
/// implementada como una dependencia singleton.
/// </summary>
public interface IGetAppId : ISingletonDependency {
    /// <summary>
    /// Obtiene el identificador de la aplicación.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de la aplicación.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para recuperar el ID único asociado a la aplicación, 
    /// que puede ser utilizado para la identificación en servicios externos o 
    /// para la configuración interna.
    /// </remarks>
    string GetAppId();
}
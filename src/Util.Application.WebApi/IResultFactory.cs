using Util.Dependency;

namespace Util.Applications; 

/// <summary>
/// Define una interfaz para la fábrica de resultados.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISingletonDependency"/> lo que indica que su implementación 
/// debe ser un singleton dentro del contenedor de inyección de dependencias.
/// </remarks>
public interface IResultFactory : ISingletonDependency {
    /// <summary>
    /// Crea un resultado de acción que encapsula un código, un mensaje, datos y un código de estado HTTP.
    /// </summary>
    /// <param name="code">El código que representa el resultado de la operación.</param>
    /// <param name="message">Un mensaje que describe el resultado de la operación.</param>
    /// <param name="data">Los datos asociados con el resultado, que pueden ser de cualquier tipo.</param>
    /// <param name="httpStatusCode">El código de estado HTTP que se debe devolver. Si es nulo, se utilizará un código de estado predeterminado.</param>
    /// <param name="options">Opciones de serialización JSON que se utilizarán al serializar los datos.</param>
    /// <returns>Un objeto <see cref="IActionResult"/> que representa el resultado de la acción.</returns>
    /// <remarks>
    /// Este método es útil para devolver respuestas estructuradas desde controladores en aplicaciones ASP.NET.
    /// Permite incluir información adicional sobre el resultado de la operación, facilitando la comunicación con el cliente.
    /// </remarks>
    IActionResult CreateResult( string code, string message, dynamic data, int? httpStatusCode, JsonSerializerOptions options );
}
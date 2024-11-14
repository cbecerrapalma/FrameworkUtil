using Util.Validation;

namespace Util.Applications.Dtos; 

/// <summary>
/// Interfaz que representa una solicitud que debe ser validada.
/// Esta interfaz hereda de <see cref="IValidation"/> para incluir funcionalidades de validación.
/// </summary>
public interface IRequest : IValidation {
}
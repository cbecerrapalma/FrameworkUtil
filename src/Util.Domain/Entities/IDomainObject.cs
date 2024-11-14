using Util.Validation;

namespace Util.Domain.Entities; 

/// <summary>
/// Interfaz que representa un objeto de dominio en el sistema.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IValidation"/> para incluir la funcionalidad de validación.
/// Los objetos que implementen esta interfaz deben representar entidades del dominio y cumplir con las reglas de negocio.
/// </remarks>
public interface IDomainObject : IValidation {
}
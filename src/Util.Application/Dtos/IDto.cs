using Util.Data;

namespace Util.Applications.Dtos; 

/// <summary>
/// Interfaz que representa un objeto de transferencia de datos (DTO).
/// Esta interfaz hereda de <see cref="IRequest"/> e <see cref="IDataKey"/>.
/// </summary>
/// <remarks>
/// Los DTO son utilizados para transferir datos entre procesos, 
/// y esta interfaz asegura que los objetos que la implementen 
/// cumplan con las características de un request y tengan una clave de datos.
/// </remarks>
public interface IDto : IRequest, IDataKey {
}
using Util.Aop;
using Util.Dependency;

namespace Util.Domain.Services; 

/// <summary>
/// Define un servicio de dominio que se utiliza para encapsular la lógica de negocio 
/// y las operaciones relacionadas con el dominio.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IScopeDependency"/> y <see cref="IAopProxy"/> 
/// para proporcionar características de inyección de dependencias y 
/// programación orientada a aspectos, respectivamente.
/// </remarks>
public interface IDomainService : IScopeDependency, IAopProxy {
}
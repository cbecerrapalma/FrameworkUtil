using Util.Aop;
using Util.Dependency;

namespace Util.Applications; 

/// <summary>
/// Define un servicio que se puede utilizar en el contexto de la inyección de dependencias.
/// Esta interfaz hereda de <see cref="IScopeDependency"/> y <see cref="IAopProxy"/>.
/// </summary>
/// <remarks>
/// Los servicios que implementan esta interfaz deben proporcionar funcionalidad específica
/// y pueden ser utilizados en un entorno que soporte la inyección de dependencias y el
/// uso de proxies orientados a aspectos.
/// </remarks>
public interface IService : IScopeDependency, IAopProxy {
}
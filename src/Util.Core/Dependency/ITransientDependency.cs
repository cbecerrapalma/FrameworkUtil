namespace Util.Dependency; 

/// <summary>
/// Interfaz que marca una dependencia como transitoria.
/// </summary>
/// <remarks>
/// Las implementaciones de esta interfaz son instancias que se crean cada vez que se solicitan.
/// Esto es útil para servicios que no deben ser compartidos entre diferentes partes de la aplicación.
/// </remarks>
public interface ITransientDependency {
}
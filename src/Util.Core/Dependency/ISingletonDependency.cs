namespace Util.Dependency; 

/// <summary>
/// Interfaz que marca una clase como una dependencia singleton.
/// </summary>
/// <remarks>
/// Las clases que implementan esta interfaz son instanciadas una sola vez durante el ciclo de vida de la aplicación.
/// Esto es útil para servicios que deben mantener un estado compartido o que son costosos de crear.
/// </remarks>
public interface ISingletonDependency {
}
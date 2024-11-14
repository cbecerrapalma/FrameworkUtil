using Util.Helpers;

namespace Util.Reflections; 

/// <summary>
/// Proporciona una implementación de <see cref="ITypeFinder"/> 
/// que busca tipos en el dominio de la aplicación.
/// </summary>
public class AppDomainTypeFinder : ITypeFinder {
    private readonly IAssemblyFinder _assemblyFinder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AppDomainTypeFinder"/>.
    /// </summary>
    /// <param name="assemblyFinder">El objeto que se utiliza para encontrar ensamblados.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="assemblyFinder"/> es <c>null</c>.</exception>
    public AppDomainTypeFinder( IAssemblyFinder assemblyFinder ) {
        _assemblyFinder = assemblyFinder ?? throw new ArgumentNullException( nameof( assemblyFinder ) );
    }

    /// <summary>
    /// Busca y devuelve una lista de tipos asociados al tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea encontrar información.</typeparam>
    /// <returns>Una lista de tipos que coinciden con el tipo especificado.</returns>
    public List<Type> Find<T>() {
        return Find( typeof( T ) );
    }

    /// <summary>
    /// Obtiene una lista de ensamblados (assemblies) disponibles.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="Assembly"/> que representan los ensamblados encontrados.
    /// </returns>
    public List<Assembly> GetAssemblies() {
        return _assemblyFinder.Find();
    }

    /// <summary>
    /// Busca todos los tipos que implementan el tipo especificado.
    /// </summary>
    /// <param name="findType">El tipo del cual se desean encontrar las implementaciones.</param>
    /// <returns>
    /// Una lista de tipos que implementan el tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Reflection"/> para realizar la búsqueda en los ensamblados disponibles.
    /// Asegúrese de que los ensamblados estén correctamente cargados antes de llamar a este método.
    /// </remarks>
    public List<Type> Find( Type findType ) {
        return Reflection.FindImplementTypes( findType, GetAssemblies()?.ToArray() );
    }
}
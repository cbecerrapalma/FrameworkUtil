using Util.Dependency;

namespace Util.Tenants.Resolvers;

/// <summary>
/// Interfaz que define las operaciones para el almacenamiento de dominios de inquilinos.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su ciclo de vida es transitorio.
/// </remarks>
public interface ITenantDomainStore : ITransientDependency {
    /// <summary>
    /// Obtiene un diccionario de pares clave-valor de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es un diccionario
    /// que contiene pares de cadenas, donde la clave y el valor son de tipo <see cref="string"/>.
    /// </returns>
    /// <remarks>
    /// Este método permite recuperar datos de manera no bloqueante, lo que es útil en aplicaciones
    /// que requieren una interfaz de usuario receptiva o en escenarios de alta concurrencia.
    /// </remarks>
    /// <seealso cref="IDictionary{TKey, TValue}"/>
    Task<IDictionary<string, string>> GetAsync();
}
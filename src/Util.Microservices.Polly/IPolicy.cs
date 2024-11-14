using Util.Dependency;

namespace Util.Microservices; 

/// <summary>
/// Define una interfaz para las políticas que deben ser implementadas por las clases que gestionan la lógica de negocio.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/>, lo que indica que las implementaciones de esta interfaz
/// deben ser registradas como dependencias transitorias en el contenedor de inyección de dependencias.
/// </remarks>
public interface IPolicy : ITransientDependency {
    /// <summary>
    /// Obtiene una instancia de un manejador de políticas de reintento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicyHandler"/> que permite gestionar políticas de reintento.
    /// </returns>
    /// <remarks>
    /// Este método es útil para implementar lógica de reintento en operaciones que pueden fallar temporalmente.
    /// </remarks>
    IRetryPolicyHandler Retry();
    /// <summary>
    /// Crea una política de reintento que se ejecutará un número específico de veces en caso de fallos.
    /// </summary>
    /// <param name="count">El número de veces que se intentará ejecutar la operación antes de fallar.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicyHandler"/> que implementa la lógica de reintento.</returns>
    /// <remarks>
    /// Esta función es útil para manejar operaciones que pueden fallar temporalmente,
    /// como llamadas a servicios externos o accesos a bases de datos.
    /// </remarks>
    /// <seealso cref="IRetryPolicyHandler"/>
    IRetryPolicyHandler Retry( int count );
}
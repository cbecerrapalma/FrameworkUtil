using Util.Dependency;

namespace Util.Tenants.Resolvers;

/// <summary>
/// Representa un atributo que indica la inversión de control (IoC) con un valor específico.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para marcar clases o métodos que deben ser gestionados por un contenedor de inversión de control.
/// </remarks>
/// <param name="value">El valor que indica el nivel de inversión de control.</param>
[Ioc(-9)]
public class NullTenantDomainStore : ITenantDomainStore {
    private readonly IDictionary<string, string> _result;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NullTenantDomainStore"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea un diccionario vacío que se utilizará para almacenar datos de dominio de inquilinos.
    /// </remarks>
    public NullTenantDomainStore() {
        _result = new Dictionary<string, string>();
    }

    public static readonly ITenantDomainStore Instance = new NullTenantDomainStore();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene de manera asíncrona un diccionario de pares clave-valor.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene un diccionario con claves y valores de tipo <see cref="string"/>.
    /// </returns>
    /// <remarks>
    /// Este método devuelve un diccionario que se ha inicializado previamente. 
    /// La operación es sincrónica en su implementación actual, ya que utiliza <see cref="Task.FromResult"/>.
    /// </remarks>
    public Task<IDictionary<string, string>> GetAsync() {
        return Task.FromResult(_result);
    }
}
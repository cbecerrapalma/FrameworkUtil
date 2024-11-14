using Util.Tenants.Resolvers;

namespace Util.Tenants;

/// <summary>
/// Representa las opciones de configuración para un inquilino.
/// </summary>
public class TenantOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece la clave del inquilino en el valor predeterminado y 
    /// configura una colección de resolutores de inquilinos con diferentes prioridades.
    /// </remarks>
    public TenantOptions() {
        TenantKey = DefaultTenantKey;
        Resolvers = new TenantResolverCollection {
            new HeaderTenantResolver{ Priority = 9 },
            new QueryStringTenantResolver{ Priority = 7 },
            new CookieTenantResolver{ Priority = 5 }
        };
    }

    public static readonly TenantOptions Null = new();

    public const string DefaultTenantKey = "x-tenant-id";

    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si se permite el uso de múltiples bases de datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar si la aplicación puede conectarse a más de una base de datos al mismo tiempo.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se permiten múltiples bases de datos; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsAllowMultipleDatabase { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del inquilino predeterminado.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador del inquilino predeterminado.
    /// </value>
    public string DefaultTenantId { get; set; }

    /// <summary>
    /// Obtiene o establece la clave del inquilino.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para identificar de manera única a un inquilino en un sistema multitenencia.
    /// </remarks>
    /// <value>
    /// La clave del inquilino como una cadena.
    /// </value>
    public string TenantKey { get; set; }

    /// <summary>
    /// Obtiene la colección de resolutores de inquilinos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una colección de objetos que implementan la lógica 
    /// para resolver inquilinos en el contexto de la aplicación.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="TenantResolverCollection"/> que contiene los resolutores de inquilinos.
    /// </returns>
    public TenantResolverCollection Resolvers { get; }
}
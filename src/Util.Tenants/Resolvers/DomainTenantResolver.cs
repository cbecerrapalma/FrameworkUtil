namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase que resuelve el inquilino (tenant) basado en el dominio.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TenantResolverBase"/> y proporciona una implementación específica
/// para determinar el inquilino en función del dominio de la solicitud.
/// </remarks>
public class DomainTenantResolver : TenantResolverBase
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainTenantResolver"/>.
    /// </summary>
    public DomainTenantResolver()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainTenantResolver"/>.
    /// </summary>
    /// <param name="domainFormat">El formato del dominio que se utilizará para resolver inquilinos.</param>
    public DomainTenantResolver(string domainFormat)
    {
        DomainFormat = domainFormat;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainTenantResolver"/>.
    /// </summary>
    /// <param name="domainMap">Un diccionario que mapea dominios a sus respectivos inquilinos.</param>
    public DomainTenantResolver(IDictionary<string, string> domainMap)
    {
        DomainMap = domainMap;
    }

    /// <summary>
    /// Obtiene el formato del dominio.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve una cadena que representa el formato del dominio 
    /// utilizado en la aplicación. El formato puede ser utilizado para validar 
    /// o manipular nombres de dominio.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el formato del dominio.
    /// </value>
    public string DomainFormat { get; }

    /// <summary>
    /// Obtiene un diccionario que mapea dominios a sus respectivas descripciones.
    /// </summary>
    /// <remarks>
    /// Este diccionario es de solo lectura y se utiliza para almacenar pares clave-valor,
    /// donde la clave es un dominio y el valor es su descripción asociada.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey,TValue}"/> que contiene los dominios como claves y sus
    /// descripciones como valores.
    /// </returns>
    public IDictionary<string, string> DomainMap { get; }

    /// <summary>
    /// Resuelve el inquilino basado en el contexto HTTP y el host proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno es una cadena que representa el ID del inquilino, 
    /// o null si no se pudo determinar el inquilino.
    /// </returns>
    /// <remarks>
    /// Este método elimina el prefijo del dominio del host de la solicitud y, si el host está vacío, 
    /// devuelve null. De lo contrario, llama a otro método para resolver el inquilino y registra la operación.
    /// </remarks>
    /// <seealso cref="DomainTenantResolverHelper.RemoveDomainPrefix(string)"/>
    protected override async Task<string> Resolve(HttpContext context)
    {
        var host = DomainTenantResolverHelper.RemoveDomainPrefix(context.Request.Host.Value);
        if (host.IsEmpty())
            return null;
        var result = await Resolve(context, host);
        GetLog(context).LogTrace($"Ejecutar el analizador de inquilinos de dominio.,host={host},tenantId={result}");
        return result;
    }

    /// <summary>
    /// Resuelve el identificador de inquilino (tenant ID) a partir del contexto HTTP y el host proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <param name="host">El nombre del host que se utilizará para resolver el inquilino.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es el identificador de inquilino como una cadena.</returns>
    /// <remarks>
    /// Este método utiliza un servicio de almacenamiento de dominios de inquilinos para combinar un mapa de dominios 
    /// y luego resuelve el inquilino correspondiente al host dado.
    /// </remarks>
    /// <seealso cref="ITenantDomainStore"/>
    /// <seealso cref="DomainTenantResolverHelper"/>
    private async Task<string> Resolve(HttpContext context, string host)
    {
        var store = context.RequestServices.GetService<ITenantDomainStore>();
        var map = await DomainTenantResolverHelper.CombineMapAsync(DomainMap, store);
        return DomainTenantResolverHelper.ResolveTenantId(host, map, DomainFormat);
    }
}
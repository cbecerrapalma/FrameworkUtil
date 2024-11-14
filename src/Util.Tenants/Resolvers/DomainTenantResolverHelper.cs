namespace Util.Tenants.Resolvers;

/// <summary>
/// Proporciona métodos de ayuda para resolver inquilinos de dominio.
/// </summary>
public class DomainTenantResolverHelper {
    /// <summary>
    /// Resuelve el identificador del inquilino a partir de un dominio dado, un mapa de dominios y un formato de dominio.
    /// </summary>
    /// <param name="domain">El dominio del cual se desea resolver el identificador del inquilino.</param>
    /// <param name="domainMap">Un diccionario que mapea dominios a identificadores de inquilinos.</param>
    /// <param name="domainFormat">El formato que se utilizará para resolver el identificador del inquilino si no se encuentra en el mapa.</param>
    /// <returns>
    /// El identificador del inquilino correspondiente al dominio proporcionado, o null si no se puede resolver.
    /// </returns>
    /// <remarks>
    /// Este método primero verifica si el dominio está vacío. Si es así, devuelve null. 
    /// Luego, elimina el prefijo del dominio y verifica si el resultado está vacío. 
    /// Si el mapa de dominios tiene elementos, intenta resolver el identificador utilizando el mapa. 
    /// Si no se encuentra un resultado, se procesa el formato de dominio y se resuelve el identificador 
    /// utilizando el formato o dividiendo el host.
    /// </remarks>
    public static string ResolveTenantId( string domain, IDictionary<string, string> domainMap, string domainFormat ) {
        if ( domain.IsEmpty() )
            return null;
        var host = RemoveDomainPrefix( domain );
        if ( host.IsEmpty() )
            return null;
        if ( domainMap is { Count: > 0 } ) {
            var result = ResolveByMap( host, domainMap );
            if ( result.IsEmpty() == false )
                return result;
        }
        domainFormat = domainFormat.RemoveStart( "," ).RemoveEnd( "," );
        return domainFormat.IsEmpty() ? ResolveBySplit( host ): ResolveByFormat( host, domainFormat );
    }

    /// <summary>
    /// Elimina el prefijo de dominio "http://" o "https://" de una cadena de texto.
    /// </summary>
    /// <param name="domain">La cadena de texto que representa el dominio del cual se eliminarán los prefijos.</param>
    /// <returns>
    /// Una cadena de texto sin el prefijo "http://" o "https://". Si el dominio no contiene estos prefijos, se devuelve tal cual.
    /// </returns>
    public static string RemoveDomainPrefix( string domain ) {
        return domain.RemoveStart( "http://" ).RemoveStart( "https://" );
    }

    /// <summary>
    /// Resuelve un nombre de host utilizando un mapa de dominios.
    /// </summary>
    /// <param name="host">El nombre del host que se desea resolver.</param>
    /// <param name="domainMap">Un diccionario que mapea nombres de host a sus correspondientes valores.</param>
    /// <returns>
    /// Devuelve el valor asociado al nombre de host si se encuentra en el mapa; de lo contrario, devuelve null.
    /// </returns>
    private static string ResolveByMap( string host, IDictionary<string, string> domainMap ) {
        foreach ( var item in domainMap ) {
            if ( item.Key == host )
                return item.Value;
        }
        return null;
    }

    /// <summary>
    /// Resuelve un valor basado en el formato de dominio proporcionado.
    /// </summary>
    /// <param name="host">El host del cual se extraerá el valor.</param>
    /// <param name="domainFormat">El formato de dominio que se utilizará para la resolución.</param>
    /// <returns>
    /// Un string que representa el valor extraído del host según el formato de dominio, 
    /// o null si no se pudo encontrar un valor válido.
    /// </returns>
    /// <remarks>
    /// Este método itera a través de los formatos de dominio obtenidos y utiliza 
    /// un método de extracción para encontrar el valor correspondiente en el host. 
    /// Si no se encuentra un valor válido en ninguno de los formatos, se devuelve null.
    /// </remarks>
    /// <seealso cref="GetDomainFormats(string)"/>
    /// <seealso cref="Util.Helpers.String.Extract(string, string)"/>
    /// <seealso cref="RemoveDomainPrefix(string)"/>
    private static string ResolveByFormat( string host, string domainFormat ) {
        var formats = GetDomainFormats( domainFormat );
        foreach ( var format in formats ) {
            if ( format.IsEmpty() )
                continue;
            var result = Util.Helpers.String.Extract( host, RemoveDomainPrefix( format ) ).FirstOrDefault().Value;
            if ( result.IsEmpty() == false )
                return result;
        }
        return null;
    }

    /// <summary>
    /// Obtiene los formatos de dominio a partir de una cadena de texto.
    /// </summary>
    /// <param name="domainFormat">La cadena que contiene uno o más formatos de dominio, separados por comas o punto y coma.</param>
    /// <returns>
    /// Una colección de cadenas que representan los formatos de dominio extraídos de la cadena de entrada.
    /// Si no se encuentran separadores, se devuelve una colección que contiene la cadena original.
    /// </returns>
    private static IEnumerable<string> GetDomainFormats( string domainFormat ) {
        if ( domainFormat.IndexOf( ",", StringComparison.Ordinal ) >= 0 )
            return domainFormat.Split( ',' );
        if ( domainFormat.IndexOf( ";", StringComparison.Ordinal ) >= 0 )
            return domainFormat.Split( ';' );
        return new[] { domainFormat };
    }

    /// <summary>
    /// Resuelve el nombre de host dividiéndolo en partes y devuelve la primera parte si hay más de dos.
    /// </summary>
    /// <param name="host">El nombre de host que se desea dividir.</param>
    /// <returns>
    /// La primera parte del nombre de host si contiene más de dos partes, de lo contrario, devuelve null.
    /// </returns>
    private static string ResolveBySplit( string host ) {
        var items = host.Split( '.' );
        if ( items.Length > 2 )
            return items[0];
        return null;
    }

    /// <summary>
    /// Combina un mapa de dominios existente con un mapa obtenido de un almacén de dominios de inquilinos.
    /// </summary>
    /// <param name="domainMap">El mapa de dominios existente que se desea combinar.</param>
    /// <param name="store">El almacén de dominios de inquilinos desde el cual se obtendrá el nuevo mapa.</param>
    /// <returns>
    /// Un diccionario que representa el mapa de dominios combinado. Si el almacén es nulo o el mapa obtenido está vacío,
    /// se devuelve el mapa de dominios existente sin cambios.
    /// </returns>
    /// <remarks>
    /// Este método es asíncrono y se debe invocar utilizando la palabra clave <c>await</c>.
    /// Si el <paramref name="store"/> es nulo, se devuelve directamente el <paramref name="domainMap"/> sin modificaciones.
    /// Si el mapa obtenido del <paramref name="store"/> es nulo o está vacío, se devuelve el <paramref name="domainMap"/> sin cambios.
    /// Los elementos del mapa obtenido se añaden al <paramref name="domainMap"/> solo si no existen previamente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="domainMap"/> es nulo y <paramref name="store"/> no es nulo.</exception>
    /// <seealso cref="ITenantDomainStore"/>
    public static async Task<IDictionary<string, string>> CombineMapAsync( IDictionary<string, string> domainMap, ITenantDomainStore store) {
        if ( store == null )
            return domainMap;
        var map = await store.GetAsync();
        if ( map == null || map.Count == 0 )
            return domainMap;
        domainMap ??= new Dictionary<string, string>();
        foreach ( var item in map ) {
            if ( domainMap.ContainsKey( item.Key ) == false ) {
                domainMap.Add( item.Key, item.Value );
            }
        }
        return domainMap;
    }
}
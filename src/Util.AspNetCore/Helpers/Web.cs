using System.Collections.Specialized;
using System.Security.Claims;
using Util.Http;
using Util.Security.Authentication;

namespace Util.Helpers;

/// <summary>
/// Proporciona métodos y propiedades para trabajar con operaciones web.
/// </summary>
public static class Web
{

    #region HttpContextAccessor(Acceso al contexto HTTP)

    /// <summary>
    /// Obtiene o establece el acceso al contexto HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder al contexto HTTP actual, lo que facilita la obtención de información sobre la solicitud y la respuesta.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IHttpContextAccessor"/> que proporciona acceso al contexto HTTP.
    /// </value>
    public static IHttpContextAccessor HttpContextAccessor { get; set; }

    #endregion

    #region HttpContext(Contexto HTTP)

    /// <summary>
    /// Obtiene el contexto HTTP actual.
    /// </summary>
    /// <value>
    /// El contexto HTTP actual, o <c>null</c> si no está disponible.
    /// </value>
    /// <remarks>
    /// Este miembro está diseñado para ser utilizado en un entorno donde se tiene acceso a un
    /// <see cref="IHttpContextAccessor"/>. Asegúrese de que el <see cref="HttpContextAccessor"/> 
    /// esté correctamente configurado en la inyección de dependencias para evitar excepciones.
    /// </remarks>
    public static HttpContext HttpContext => HttpContextAccessor?.HttpContext;

    #endregion

    #region ServiceProvider(Proveedor de servicios)

    /// <summary>
    /// Obtiene el proveedor de servicios de la solicitud HTTP actual.
    /// </summary>
    /// <value>
    /// Un <see cref="IServiceProvider"/> que representa el proveedor de servicios asociado a la solicitud HTTP actual,
    /// o <c>null</c> si no hay un contexto HTTP disponible.
    /// </value>
    /// <remarks>
    /// Este miembro es útil para acceder a los servicios registrados en el contenedor de inyección de dependencias
    /// en el contexto de una solicitud web.
    /// </remarks>
    public static IServiceProvider ServiceProvider => HttpContext?.RequestServices;

    #endregion

    #region Request(Solicitud HTTP)

    /// <summary>
    /// Obtiene la solicitud HTTP actual.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="HttpRequest"/> que representa la solicitud HTTP actual,
    /// o <c>null</c> si el contexto HTTP no está disponible.
    /// </value>
    /// <remarks>
    /// Esta propiedad es útil para acceder a los datos de la solicitud, como los encabezados,
    /// los parámetros de la consulta y el cuerpo de la solicitud.
    /// </remarks>
    public static HttpRequest Request => HttpContext?.Request;

    #endregion

    #region Response(Respuesta HTTP)

    /// <summary>
    /// Obtiene la respuesta HTTP actual del contexto.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="HttpResponse"/> que representa la respuesta HTTP actual,
    /// o <c>null</c> si el contexto HTTP no está disponible.
    /// </value>
    public static HttpResponse Response => HttpContext?.Response;

    #endregion

    #region Body(Solicitud de texto)

    /// <summary>
    /// Obtiene el contenido del cuerpo de la solicitud como un arreglo de bytes.
    /// </summary>
    /// <returns>
    /// Un arreglo de bytes que representa el contenido del cuerpo de la solicitud.
    /// </returns>
    /// <remarks>
    /// Este método habilita el almacenamiento en búfer de la solicitud para permitir la lectura del cuerpo varias veces.
    /// Asegúrese de que el cuerpo de la solicitud no se haya leído previamente antes de acceder a esta propiedad.
    /// </remarks>
    public static byte[] Body
    {
        get
        {
            Request.EnableBuffering();
            return File.ReadToBytes(Request.Body);
        }
    }

    #endregion

    #region GetBodyAsync(Obtener el cuerpo de la solicitud)

    /// <summary>
    /// Obtiene el contenido del cuerpo de la solicitud de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, 
    /// que contiene un arreglo de bytes que representa el cuerpo de la solicitud.
    /// </returns>
    /// <remarks>
    /// Este método habilita el almacenamiento en búfer de la solicitud 
    /// para poder leer el cuerpo varias veces si es necesario.
    /// </remarks>
    public static async Task<byte[]> GetBodyAsync()
    {
        Request.EnableBuffering();
        return await File.ReadToBytesAsync(Request.Body);
    }

    #endregion

    #region Environment(Entorno del host)

    /// <summary>
    /// Obtiene el entorno de hospedaje web actual.
    /// </summary>
    /// <value>
    /// El entorno de hospedaje web, o <c>null</c> si el proveedor de servicios no está disponible.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la configuración del entorno, como el modo de desarrollo o producción.
    /// Asegúrese de que el <see cref="ServiceProvider"/> esté configurado correctamente antes de acceder a esta propiedad.
    /// </remarks>
    public static IWebHostEnvironment Environment => ServiceProvider?.GetService<IWebHostEnvironment>();

    #endregion

    #region User(Usuario de seguridad actual)

    /// <summary>
    /// Obtiene el objeto <see cref="ClaimsPrincipal"/> que representa al usuario actual.
    /// </summary>
    /// <remarks>
    /// Si el contexto HTTP actual no está disponible o si no hay un usuario autenticado,
    /// se devuelve una instancia de <see cref="UnauthenticatedPrincipal"/>.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="ClaimsPrincipal"/> que representa al usuario actual, o una instancia
    /// de <see cref="UnauthenticatedPrincipal"/> si no hay un usuario autenticado.
    /// </returns>
    public static ClaimsPrincipal User
    {
        get
        {
            if (HttpContext?.User is { } principal)
                return principal;
            return UnauthenticatedPrincipal.Instance;
        }
    }

    #endregion

    #region Identity(Identificación del usuario actual)

    /// <summary>
    /// Obtiene la identidad de usuario actual como un objeto <see cref="ClaimsIdentity"/>.
    /// </summary>
    /// <remarks>
    /// Si el usuario actual no está autenticado, se devuelve una instancia de <see cref="UnauthenticatedIdentity"/>.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="ClaimsIdentity"/> que representa la identidad del usuario actual,
    /// o una instancia de <see cref="UnauthenticatedIdentity"/> si el usuario no está autenticado.
    /// </returns>
    public static ClaimsIdentity Identity
    {
        get
        {
            if (User.Identity is ClaimsIdentity identity)
                return identity;
            return UnauthenticatedIdentity.Instance;
        }
    }

    #endregion

    #region Client(Cliente HTTP)

    /// <summary>
    /// Obtiene una instancia de <see cref="IHttpClient"/> utilizando el contenedor de inversión de control (IoC).
    /// </summary>
    /// <remarks>
    /// Esta propiedad es estática y permite acceder a una implementación de <see cref="IHttpClient"/> 
    /// sin necesidad de crearla manualmente. Se utiliza comúnmente para realizar solicitudes HTTP 
    /// en aplicaciones que utilizan un patrón de diseño basado en IoC.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IHttpClient"/> que puede ser utilizada para realizar operaciones HTTP.
    /// </returns>
    /// <seealso cref="IHttpClient"/>
    /// <seealso cref="Ioc"/>
    public static IHttpClient Client => Ioc.Create<IHttpClient>();

    #endregion

    #region GetPhysicalPath(Obtener la ruta física)

    /// <summary>
    /// Obtiene la ruta física correspondiente a una ruta relativa.
    /// </summary>
    /// <param name="relativePath">La ruta relativa que se desea convertir a una ruta física.</param>
    /// <returns>La ruta física que corresponde a la ruta relativa proporcionada.</returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="Common.GetPhysicalPath(string, string)"/> 
    /// para realizar la conversión, utilizando la ruta raíz del contenido del entorno.
    /// </remarks>
    public static string GetPhysicalPath(string relativePath)
    {
        return Common.GetPhysicalPath(relativePath, Environment.ContentRootPath);
    }

    #endregion

    #region GetFiles(Obtener conjunto de archivos del cliente)

    /// <summary>
    /// Obtiene una lista de archivos subidos a través de una solicitud HTTP.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="IFormFile"/> que representan los archivos subidos.
    /// Si no hay archivos, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay archivos en la solicitud y solo agrega aquellos que tienen un tamaño mayor a cero.
    /// </remarks>
    public static List<IFormFile> GetFiles()
    {
        var result = new List<IFormFile>();
        var files = Request.Form.Files;
        if (files.Count == 0)
            return result;
        result.AddRange(files.Where(file => file?.Length > 0));
        return result;
    }

    #endregion

    #region GetFile(Obtener archivos del cliente)

    /// <summary>
    /// Obtiene el primer archivo de la lista de archivos disponibles.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IFormFile"/> que representa el primer archivo si existe; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    public static IFormFile GetFile()
    {
        var files = GetFiles();
        return files.Count == 0 ? null : files[0];
    }

    #endregion

    #region GetParam(Obtener parámetros de la solicitud)

    /// <summary>
    /// Obtiene el valor de un parámetro de la solicitud actual, buscando en las consultas, formularios y encabezados.
    /// </summary>
    /// <param name="name">El nombre del parámetro que se desea obtener.</param>
    /// <returns>
    /// Devuelve el valor del parámetro si se encuentra; de lo contrario, devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método primero verifica si el nombre del parámetro está vacío. Si es así, retorna una cadena vacía.
    /// Luego, comprueba si la solicitud actual es nula. Si lo es, también retorna una cadena vacía.
    /// A continuación, busca el valor del parámetro en la colección de consultas y, si no se encuentra, 
    /// busca en la colección de formularios. Si aún no se encuentra, finalmente busca en los encabezados de la solicitud.
    /// </remarks>
    public static string GetParam(string name)
    {
        if (name.IsEmpty())
            return string.Empty;
        if (Request == null)
            return string.Empty;
        string result = Request.Query[name];
        if (result.IsEmpty() == false)
            return result;
        result = Request.Form[name];
        if (result.IsEmpty() == false)
            return result;
        return Request.Headers[name];
    }

    #endregion

    #region Host(Obtener host)

    /// <summary>
    /// Obtiene la URL del host actual, incluyendo el esquema (http o https) y el nombre del host.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URL del host en el formato "esquema://host".
    /// </value>
    /// <remarks>
    /// Este valor se construye utilizando la propiedad <see cref="Request"/>. 
    /// Si <see cref="Request"/> es nulo, el resultado será una cadena vacía.
    /// </remarks>
    public static string Host => $"{Request?.Scheme}://{Request?.Host.Value}";

    #endregion

    #region Url(Dirección de solicitud)

    /// <summary>
    /// Obtiene la URL actual de la solicitud.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URL completa de la solicitud actual, 
    /// o <c>null</c> si la solicitud es <c>null</c>.
    /// </value>
    /// <remarks>
    /// Este miembro es estático y se puede acceder sin instanciar la clase.
    /// Asegúrese de que la solicitud esté disponible antes de acceder a esta propiedad.
    /// </remarks>
    public static string Url => Request?.GetDisplayUrl();

    #endregion

    #region UrlEncode(Codificación de URL)

    /// <summary>
    /// Codifica una cadena de URL utilizando la codificación especificada.
    /// </summary>
    /// <param name="url">La cadena de URL que se desea codificar.</param>
    /// <param name="isUpper">Indica si el resultado debe estar en mayúsculas. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// Una cadena que representa la URL codificada.
    /// </returns>
    /// <seealso cref="UrlEncode(string, Encoding, bool)"/>
    public static string UrlEncode(string url, bool isUpper = false)
    {
        return UrlEncode(url, Encoding.UTF8, isUpper);
    }

    /// <summary>
    /// Codifica una URL utilizando el conjunto de caracteres especificado.
    /// </summary>
    /// <param name="url">La URL que se desea codificar.</param>
    /// <param name="encoding">El nombre del conjunto de caracteres que se utilizará para la codificación. Si está vacío o es nulo, se utilizará "UTF-8".</param>
    /// <param name="isUpper">Indica si el resultado debe estar en mayúsculas. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// Una cadena que representa la URL codificada.
    /// </returns>
    /// <remarks>
    /// Este método es útil para asegurar que una URL sea segura para su transmisión a través de la red, 
    /// convirtiendo caracteres especiales en su representación codificada.
    /// </remarks>
    /// <seealso cref="UrlEncode(string, Encoding, bool)"/>
    public static string UrlEncode(string url, string encoding, bool isUpper = false)
    {
        encoding = string.IsNullOrWhiteSpace(encoding) ? "UTF-8" : encoding;
        return UrlEncode(url, Encoding.GetEncoding(encoding), isUpper);
    }

    /// <summary>
    /// Codifica una URL utilizando la codificación especificada.
    /// </summary>
    /// <param name="url">La URL que se desea codificar.</param>
    /// <param name="encoding">La codificación que se utilizará para la codificación de la URL.</param>
    /// <param name="isUpper">Indica si el resultado debe estar en mayúsculas. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// La URL codificada como una cadena. Si <paramref name="isUpper"/> es <c>true</c>, la cadena resultante estará en mayúsculas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="HttpUtility.UrlEncode"/> para realizar la codificación de la URL.
    /// Si se requiere que el resultado esté en mayúsculas, se invoca el método <see cref="GetUpperEncode"/>.
    /// </remarks>
    public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
    {
        var result = HttpUtility.UrlEncode(url, encoding);
        if (isUpper == false)
            return result;
        return GetUpperEncode(result);
    }

    /// <summary>
    /// Convierte los caracteres de una cadena codificada en formato URL a mayúsculas,
    /// específicamente aquellos que siguen a un símbolo de porcentaje ("%") que indica
    /// el inicio de una secuencia de codificación.
    /// </summary>
    /// <param name="encode">La cadena de texto que contiene la codificación a procesar.</param>
    /// <returns>Una nueva cadena con los caracteres apropiadamente convertidos a mayúsculas.</returns>
    /// <remarks>
    /// Este método busca el símbolo de porcentaje en la cadena y convierte a mayúsculas
    /// los dos caracteres que siguen a cada símbolo encontrado. Si no hay caracteres
    /// después del símbolo de porcentaje, no se realiza ninguna conversión.
    /// </remarks>
    private static string GetUpperEncode(string encode)
    {
        var result = new StringBuilder();
        int index = int.MinValue;
        for (int i = 0; i < encode.Length; i++)
        {
            string character = encode[i].ToString();
            if (character == "%")
                index = i;
            if (i - index == 1 || i - index == 2)
                character = character.ToUpper();
            result.Append(character);
        }
        return result.ToString();
    }

    #endregion

    #region UrlDecode(Decodificación de URL)

    /// <summary>
    /// Decodifica una cadena de texto que representa una URL codificada.
    /// </summary>
    /// <param name="url">La cadena de texto que contiene la URL codificada que se desea decodificar.</param>
    /// <returns>
    /// Una cadena de texto que representa la URL decodificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="HttpUtility"/> para realizar la decodificación.
    /// Asegúrese de que la cadena de entrada esté correctamente codificada antes de llamar a este método.
    /// </remarks>
    public static string UrlDecode(string url)
    {
        return HttpUtility.UrlDecode(url);
    }

    /// <summary>
    /// Decodifica una cadena de URL utilizando la codificación especificada.
    /// </summary>
    /// <param name="url">La cadena de URL que se desea decodificar.</param>
    /// <param name="encoding">La codificación que se utilizará para decodificar la cadena.</param>
    /// <returns>
    /// La cadena decodificada de la URL.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="HttpUtility"/> para realizar la decodificación.
    /// Asegúrese de que la cadena de entrada esté correctamente codificada antes de llamar a este método.
    /// </remarks>
    public static string UrlDecode(string url, Encoding encoding)
    {
        return HttpUtility.UrlDecode(url, encoding);
    }

    #endregion

    #region ParseQueryString(Convertir la cadena de consulta en un conjunto de pares clave-valor)

    /// <summary>
    /// Analiza una cadena de consulta y devuelve una colección de pares clave-valor.
    /// </summary>
    /// <param name="query">La cadena de consulta que se desea analizar. Debe estar en el formato adecuado para que se pueda descomponer en pares clave-valor.</param>
    /// <returns>
    /// Una colección de pares clave-valor que representa los parámetros de la cadena de consulta.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="HttpUtility"/> para realizar el análisis de la cadena de consulta.
    /// </remarks>
    public static NameValueCollection ParseQueryString(string query)
    {
        return HttpUtility.ParseQueryString(query);
    }

    /// <summary>
    /// Analiza una cadena de consulta y devuelve una colección de pares clave-valor.
    /// </summary>
    /// <param name="query">La cadena de consulta que se va a analizar. Debe estar en el formato de una URL.</param>
    /// <param name="encoding">La codificación que se utilizará para decodificar los valores de la cadena de consulta.</param>
    /// <returns>
    /// Una colección de pares clave-valor que representa los parámetros de la cadena de consulta.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="HttpUtility.ParseQueryString"/> para realizar el análisis.
    /// </remarks>
    public static NameValueCollection ParseQueryString(string query, Encoding encoding)
    {
        return HttpUtility.ParseQueryString(query, encoding);
    }

    #endregion

    #region DownloadAsync(Descargar)

    /// <summary>
    /// Descarga un archivo de forma asíncrona utilizando la ruta y el nombre de archivo especificados.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se desea descargar.</param>
    /// <param name="fileName">El nombre del archivo que se desea descargar.</param>
    /// <returns>Una tarea que representa la operación de descarga asíncrona.</returns>
    /// <remarks>
    /// Este método llama a otra sobrecarga de <see cref="DownloadFileAsync(string, string, Encoding)"/> 
    /// utilizando la codificación UTF-8 por defecto.
    /// </remarks>
    public static async Task DownloadFileAsync(string filePath, string fileName)
    {
        await DownloadFileAsync(filePath, fileName, Encoding.UTF8);
    }

    /// <summary>
    /// Descarga un archivo de forma asíncrona utilizando la ruta y el nombre del archivo especificados.
    /// </summary>
    /// <param name="filePath">La ruta completa del archivo que se desea descargar.</param>
    /// <param name="fileName">El nombre con el que se guardará el archivo descargado.</param>
    /// <param name="encoding">La codificación que se utilizará para el archivo descargado.</param>
    /// <returns>Una tarea que representa la operación asíncrona de descarga del archivo.</returns>
    /// <remarks>
    /// Este método lee el contenido del archivo especificado por <paramref name="filePath"/> 
    /// y lo descarga utilizando el nombre proporcionado en <paramref name="fileName"/> 
    /// y la codificación especificada en <paramref name="encoding"/>.
    /// </remarks>
    /// <seealso cref="DownloadAsync(byte[], string, Encoding)"/>
    public static async Task DownloadFileAsync(string filePath, string fileName, Encoding encoding)
    {
        var bytes = File.ReadToBytes(filePath);
        await DownloadAsync(bytes, fileName, encoding);
    }

    /// <summary>
    /// Descarga un flujo de datos de forma asíncrona y lo guarda en un archivo con el nombre especificado.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a descargar.</param>
    /// <param name="fileName">El nombre del archivo donde se guardará el flujo descargado.</param>
    /// <returns>Una tarea que representa la operación asíncrona de descarga.</returns>
    /// <remarks>
    /// Este método utiliza una codificación UTF-8 por defecto para guardar el contenido del flujo.
    /// </remarks>
    /// <seealso cref="DownloadAsync(Stream, string, Encoding)"/>
    public static async Task DownloadAsync(Stream stream, string fileName)
    {
        await DownloadAsync(stream, fileName, Encoding.UTF8);
    }

    /// <summary>
    /// Descarga un flujo de datos y lo guarda en un archivo con el nombre y la codificación especificados.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a descargar.</param>
    /// <param name="fileName">El nombre del archivo donde se guardarán los datos descargados.</param>
    /// <param name="encoding">La codificación que se utilizará para guardar el archivo.</param>
    /// <returns>Una tarea que representa la operación asincrónica de descarga.</returns>
    /// <remarks>
    /// Este método convierte el flujo en un arreglo de bytes y luego llama a otro método de descarga
    /// que maneja la lógica de guardar esos bytes en un archivo.
    /// </remarks>
    /// <seealso cref="DownloadAsync(byte[], string, Encoding)"/>
    public static async Task DownloadAsync(Stream stream, string fileName, Encoding encoding)
    {
        var bytes = await File.ToBytesAsync(stream);
        await DownloadAsync(bytes, fileName, encoding);
    }

    /// <summary>
    /// Descarga un archivo de forma asíncrona utilizando un arreglo de bytes y un nombre de archivo.
    /// </summary>
    /// <param name="bytes">El arreglo de bytes que representa el contenido del archivo a descargar.</param>
    /// <param name="fileName">El nombre del archivo que se utilizará al descargar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de descarga.</returns>
    /// <remarks>
    /// Este método llama a otra sobrecarga de <see cref="DownloadAsync(byte[], string, Encoding)"/> 
    /// utilizando la codificación UTF-8 por defecto.
    /// </remarks>
    public static async Task DownloadAsync(byte[] bytes, string fileName)
    {
        await DownloadAsync(bytes, fileName, Encoding.UTF8);
    }

    /// <summary>
    /// Descarga un archivo de forma asíncrona utilizando un arreglo de bytes.
    /// </summary>
    /// <param name="bytes">El arreglo de bytes que representa el contenido del archivo a descargar.</param>
    /// <param name="fileName">El nombre del archivo que se descargará. Se eliminarán los espacios en blanco y se codificará según la codificación especificada.</param>
    /// <param name="encoding">La codificación que se utilizará para codificar el nombre del archivo.</param>
    /// <returns>Una tarea que representa la operación asíncrona de descarga del archivo.</returns>
    /// <remarks>
    /// Este método establece el tipo de contenido de la respuesta como "application/octet-stream" 
    /// y añade los encabezados necesarios para la descarga del archivo.
    /// Si el arreglo de bytes es nulo o está vacío, el método no realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="System.Threading.Tasks.Task"/>
    public static async Task DownloadAsync(byte[] bytes, string fileName, Encoding encoding)
    {
        if (bytes == null || bytes.Length == 0)
            return;
        fileName = fileName.Replace(" ", "");
        fileName = UrlEncode(fileName, encoding);
        Response.ContentType = "application/octet-stream";
        Response.Headers.Append("Content-Disposition", $"attachment; filename={fileName}");
        Response.Headers.Append("Content-Length", bytes.Length.ToString());
        await Response.Body.WriteAsync(bytes, 0, bytes.Length);
    }

    #endregion

    #region GetCookie(Obtener Cookie)

    /// <summary>
    /// Obtiene el valor de una cookie específica a partir de su clave.
    /// </summary>
    /// <param name="key">La clave de la cookie que se desea obtener.</param>
    /// <returns>
    /// El valor de la cookie si existe; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la solicitud actual contiene cookies y devuelve el valor de la cookie
    /// correspondiente a la clave proporcionada. Si la cookie no existe, se devuelve <c>null</c>.
    /// </remarks>
    public static string GetCookie(string key)
    {
        return Request?.Cookies[key];
    }

    #endregion

    #region SetCookie(Configurar cookies)

    /// <summary>
    /// Establece una cookie en la respuesta HTTP con la clave y el valor especificados.
    /// </summary>
    /// <param name="key">La clave de la cookie que se va a establecer.</param>
    /// <param name="value">El valor de la cookie que se va a establecer.</param>
    /// <remarks>
    /// Este método utiliza el objeto Response para agregar la cookie a la respuesta. 
    /// Si el objeto Response es nulo, no se realizará ninguna acción.
    /// </remarks>
    public static void SetCookie(string key, string value)
    {
        Response?.Cookies.Append(key, value);
    }

    /// <summary>
    /// Establece una cookie en la respuesta HTTP con la clave y el valor especificados.
    /// </summary>
    /// <param name="key">La clave de la cookie que se va a establecer.</param>
    /// <param name="value">El valor de la cookie que se va a establecer.</param>
    /// <param name="options">Las opciones de configuración de la cookie, como la duración, el dominio y la ruta.</param>
    /// <remarks>
    /// Este método utiliza la respuesta HTTP actual para agregar la cookie. 
    /// Asegúrese de que la respuesta no haya sido finalizada antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="CookieOptions"/>
    public static void SetCookie(string key, string value, CookieOptions options)
    {
        Response?.Cookies.Append(key, value, options);
    }

    #endregion

    #region RemoveCookie(Eliminar cookies)

    /// <summary>
    /// Elimina una cookie del cliente utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave de la cookie que se desea eliminar.</param>
    /// <remarks>
    /// Si la cookie no existe, no se generará ningún error. Este método utiliza el objeto Response
    /// para acceder a las cookies del cliente y eliminar la cookie correspondiente.
    /// </remarks>
    /// <seealso cref="HttpResponse.Cookies"/>
    public static void RemoveCookie(string key)
    {
        Response?.Cookies.Delete(key);
    }

    /// <summary>
    /// Elimina una cookie del cliente.
    /// </summary>
    /// <param name="key">La clave de la cookie que se desea eliminar.</param>
    /// <param name="options">Opciones adicionales para la eliminación de la cookie.</param>
    /// <remarks>
    /// Este método intenta eliminar la cookie especificada del cliente.
    /// Si la cookie no existe, no se produce ningún error.
    /// </remarks>
    public static void RemoveCookie(string key, CookieOptions options)
    {
        Response?.Cookies.Delete(key, options);
    }

    #endregion
}
namespace Util.Microservices;

/// <summary>
/// Define una interfaz base para la invocación de servicios.
/// </summary>
/// <typeparam name="TServiceInvocation">El tipo de la invocación de servicio que implementa esta interfaz.</typeparam>
public interface IServiceInvocationBase<out TServiceInvocation> where TServiceInvocation : IServiceInvocationBase<TServiceInvocation> {
    /// <summary>
    /// Obtiene una instancia de <see cref="TServiceInvocation"/> asociada a un identificador de aplicación específico.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación para la cual se desea obtener el servicio.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> que representa el servicio asociado a la aplicación.</returns>
    /// <remarks>
    /// Este método permite invocar servicios específicos basados en el identificador de la aplicación proporcionado.
    /// Asegúrese de que el <paramref name="appId"/> sea válido y esté registrado en el sistema.
    /// </remarks>
    /// <seealso cref="TServiceInvocation"/>
    TServiceInvocation Service( string appId );
    /// <summary>
    /// Desempaqueta el resultado de una invocación de servicio.
    /// </summary>
    /// <param name="isUnpack">Indica si se debe desempaquetar el resultado.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> que representa el resultado desempaquetado.</returns>
    /// <remarks>
    /// Este método se utiliza para procesar el resultado de una invocación de servicio,
    /// permitiendo que el resultado sea desempaquetado en función del parámetro proporcionado.
    /// </remarks>
    TServiceInvocation UnpackResult( bool isUnpack );
    /// <summary>
    /// Establece el token de portador (Bearer Token) para la invocación del servicio.
    /// </summary>
    /// <param name="token">El token de portador que se utilizará para la autenticación.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> configurada con el token de portador.</returns>
    /// <remarks>
    /// Este método es útil para autenticar solicitudes a servicios que requieren un token de acceso.
    /// Asegúrese de que el token proporcionado sea válido y no haya expirado.
    /// </remarks>
    TServiceInvocation BearerToken( string token );
    /// <summary>
    /// Establece un encabezado para la invocación del servicio.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea establecer.</param>
    /// <param name="value">El valor asociado a la clave del encabezado.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> con el encabezado establecido.</returns>
    /// <remarks>
    /// Este método permite agregar encabezados personalizados a la invocación del servicio,
    /// lo que puede ser útil para la autenticación, el seguimiento o cualquier otro propósito
    /// que requiera información adicional en la solicitud.
    /// </remarks>
    TServiceInvocation Header( string key, string value );
    /// <summary>
    /// Invoca un servicio y permite establecer encabezados personalizados.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a enviar con la invocación del servicio.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> que representa la invocación del servicio.</returns>
    /// <remarks>
    /// Este método es útil para agregar información adicional a la solicitud, como tokens de autenticación o metadatos.
    /// Asegúrese de que los encabezados proporcionados sean válidos y estén en el formato correcto.
    /// </remarks>
    TServiceInvocation Header( IDictionary<string, string> headers );
    /// <summary>
    /// Importa un encabezado basado en la clave proporcionada.
    /// </summary>
    /// <param name="key">La clave que se utilizará para importar el encabezado.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> que representa el encabezado importado.</returns>
    /// <remarks>
    /// Este método se utiliza para recuperar información asociada a la clave especificada.
    /// Asegúrese de que la clave proporcionada sea válida para evitar excepciones.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="key"/> es null.</exception>
    /// <exception cref="InvalidOperationException">Se lanza si ocurre un error durante la importación del encabezado.</exception>
    TServiceInvocation ImportHeader( string key );
    /// <summary>
    /// Importa un encabezado utilizando una colección de claves proporcionadas.
    /// </summary>
    /// <param name="keys">Una colección de cadenas que representan las claves a importar.</param>
    /// <returns>Un objeto de tipo <see cref="TServiceInvocation"/> que representa el resultado de la operación de importación.</returns>
    /// <remarks>
    /// Este método permite importar encabezados a partir de un conjunto de claves. 
    /// Asegúrese de que las claves proporcionadas sean válidas y estén en el formato correcto.
    /// </remarks>
    /// <seealso cref="TServiceInvocation"/>
    TServiceInvocation ImportHeader( IEnumerable<string> keys );
    /// <summary>
    /// Elimina un encabezado específico de la invocación del servicio.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea eliminar.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> con el encabezado eliminado.</returns>
    /// <remarks>
    /// Este método permite gestionar los encabezados de la invocación del servicio,
    /// asegurando que solo los encabezados necesarios estén presentes en la solicitud.
    /// </remarks>
    /// <seealso cref="AddHeader(string, string)"/>
    TServiceInvocation RemoveHeader( string key );
    /// <summary>
    /// Establece una acción que se ejecutará cuando cambie el estado del servicio.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="string"/> como entrada y devuelve un <see cref="ServiceState"/> que representa el nuevo estado del servicio.</param>
    /// <returns>Una instancia de <see cref="TServiceInvocation"/> que permite encadenar otras invocaciones de servicio.</returns>
    /// <remarks>
    /// Este método permite registrar una acción personalizada que se invocará cada vez que el estado del servicio cambie.
    /// Asegúrese de que la función proporcionada maneje correctamente los posibles estados del servicio.
    /// </remarks>
    /// <seealso cref="ServiceState"/>
    /// <seealso cref="TServiceInvocation"/>
    TServiceInvocation OnState( Func<string, ServiceState> action );
}
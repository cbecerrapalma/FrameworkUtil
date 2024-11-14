using Util.Microservices.Dapr.ServiceInvocations;

namespace Util.Microservices.Dapr;

/// <summary>
/// Representa las opciones de invocación de un servicio.
/// </summary>
public class ServiceInvocationOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ServiceInvocationOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece los filtros de solicitud y respuesta como listas vacías,
    /// inicializa las claves de encabezado que se importarán y establece la opción 
    /// para desempaquetar el resultado en verdadero.
    /// </remarks>
    public ServiceInvocationOptions() {
        RequestFilters = new List<IRequestFilter>();
        ResponseFilters = new List<IResponseFilter>();
        ImportHeaderKeys = new List<string> {
            "Authorization",
            "x-correlation-id",
            "Content-Language"
        };
        IsUnpackResult = true;
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si el resultado de la descompresión es exitoso.
    /// </summary>
    /// <value>
    /// <c>true</c> si el resultado de la descompresión es exitoso; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsUnpackResult { get; set; }
    /// <summary>
    /// Representa una función que se utiliza para obtener el estado de un servicio basado en una cadena de entrada.
    /// </summary>
    /// <value>
    /// Una función que toma un <see cref="string"/> como parámetro y devuelve un <see cref="ServiceState"/>.
    /// </value>
    public Func<string, ServiceState> OnState { get; set; }
    /// <summary>
    /// Representa un delegado que se ejecuta antes de la invocación de un servicio.
    /// </summary>
    /// <remarks>
    /// Este delegado permite definir una acción personalizada que se ejecutará antes de que se realice la invocación del servicio.
    /// Puede ser utilizado para realizar tareas como la validación de argumentos, la configuración del contexto, 
    /// o el registro de información de diagnóstico.
    /// </remarks>
    /// <value>
    /// Una función que toma un objeto de tipo <see cref="ServiceInvocationArgument"/> y devuelve un <see cref="Task"/>.
    /// </value>
    public Func<ServiceInvocationArgument, Task> OnBefore { get; set; }
    /// <summary>
    /// Representa una función que se ejecuta cuando una operación se completa con éxito.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite asignar una función que se invocará cuando la operación relacionada
    /// se ejecute correctamente, proporcionando un argumento de tipo <see cref="ServiceInvocationArgument"/>.
    /// </remarks>
    /// <value>
    /// Una función que toma un argumento de tipo <see cref="ServiceInvocationArgument"/> y devuelve un <see cref="Task"/>.
    /// </value>
    public Func<ServiceInvocationArgument, Task> OnSuccess { get; set; }
    /// <summary>
    /// Representa una función que se ejecuta cuando una invocación de servicio falla.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir un comportamiento personalizado que se ejecutará en caso de que la invocación de servicio no se complete exitosamente.
    /// </remarks>
    /// <value>
    /// Una función que toma un argumento de tipo <see cref="ServiceInvocationArgument"/> y devuelve un <see cref="Task"/>.
    /// </value>
    public Func<ServiceInvocationArgument, Task> OnFail { get; set; }
    /// <summary>
    /// Representa un delegado que se ejecuta cuando se detecta un acceso no autorizado.
    /// </summary>
    /// <remarks>
    /// Este delegado permite definir una acción personalizada que se llevará a cabo
    /// cuando se produzca un intento de acceso no autorizado, proporcionando
    /// un argumento de tipo <see cref="ServiceInvocationArgument"/> que puede contener
    /// información relevante sobre la invocación del servicio.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="Func{T, TResult}"/> que toma un argumento de tipo
    /// <see cref="ServiceInvocationArgument"/> y devuelve un <see cref="Task"/>.
    /// </value>
    public Func<ServiceInvocationArgument, Task> OnUnauthorized { get; set; }
    /// <summary>
    /// Representa un delegado que se invoca cuando una operación se completa.
    /// </summary>
    /// <remarks>
    /// Este delegado acepta un argumento de tipo <see cref="ServiceInvocationArgument"/> 
    /// y devuelve una tarea que representa la operación asincrónica.
    /// </remarks>
    /// <param name="arg">El argumento de la invocación del servicio que se pasa al completar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public Func<ServiceInvocationArgument, Task> OnComplete { get; set; }
    /// <summary>
    /// Obtiene o establece una lista de filtros de solicitud.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite agregar o modificar filtros que se aplican a las solicitudes.
    /// Los filtros de solicitud son utilizados para interceptar y manipular las solicitudes antes de que sean procesadas.
    /// </remarks>
    /// <value>
    /// Una lista de objetos que implementan la interfaz <see cref="IRequestFilter"/>.
    /// </value>
    public IList<IRequestFilter> RequestFilters { get; set; }
    /// <summary>
    /// Obtiene o establece una lista de filtros de respuesta.
    /// </summary>
    /// <remarks>
    /// Los filtros de respuesta se utilizan para modificar o procesar la respuesta antes de que sea enviada al cliente.
    /// Esta propiedad permite agregar, eliminar o acceder a los filtros de respuesta que se aplicarán.
    /// </remarks>
    /// <value>
    /// Una lista que implementa <see cref="IResponseFilter"/>.
    /// </value>
    public IList<IResponseFilter> ResponseFilters { get; set; }
    /// <summary>
    /// Obtiene o establece una lista de claves de encabezado que se importarán.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar las claves de encabezado que se utilizarán durante el proceso de importación.
    /// </remarks>
    /// <value>
    /// Una lista de cadenas que representa las claves de encabezado.
    /// </value>
    public IList<string> ImportHeaderKeys { get; set; }
}
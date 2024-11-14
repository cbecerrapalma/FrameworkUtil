using Util.Applications.Locks;
using Util.AspNetCore;
using Util.Helpers;
using Util.Properties;
using Util.Sessions;
using Util.SystemTextJson;

namespace Util.Applications.Filters; 

/// <summary>
/// Indica que un método debe ser tratado de una manera especial.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para marcar métodos que requieren un procesamiento específico,
/// como la validación de datos o la ejecución de lógica adicional antes o después de la llamada al método.
/// </remarks>
/// <example>
/// [MiAtributoEspecial]
/// 
[AttributeUsage( AttributeTargets.Method )]
public class LockAttribute : ActionFilterAttribute {
    /// <summary>
    /// Obtiene o establece la clave.
    /// </summary>
    /// <value>
    /// La clave como una cadena de texto.
    /// </value>
    public string Key { get; set; }
    /// <summary>
    /// Representa el tipo de bloqueo asociado a un objeto.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es <see cref="LockType.User"/>.
    /// </remarks>
    /// <value>
    /// El tipo de bloqueo, que puede ser uno de los valores definidos en <see cref="LockType"/>.
    /// </value>
    public LockType Type { get; set; } = LockType.User;
    /// <summary>
    /// Obtiene o establece el intervalo.
    /// </summary>
    /// <remarks>
    /// Este valor representa el intervalo en el que se realizan ciertas operaciones.
    /// Asegúrese de que el intervalo esté dentro de un rango válido para evitar errores.
    /// </remarks>
    /// <value>
    /// Un entero que representa el intervalo.
    /// </value>
    public int Interval { get; set; }

    /// <summary>
    /// Ejecuta la lógica de acción de forma asíncrona, aplicando un mecanismo de bloqueo para evitar 
    /// la ejecución concurrente de la misma acción.
    /// </summary>
    /// <param name="context">El contexto de la acción que se está ejecutando.</param>
    /// <param name="next">El delegado que representa la siguiente acción en la cadena de ejecución.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método se asegura de que solo una instancia de la acción se ejecute a la vez, utilizando un 
    /// mecanismo de bloqueo basado en una clave generada a partir del contexto. Si el bloqueo no se puede 
    /// adquirir, se establece el resultado del contexto con un mensaje de fallo.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="context"/> o <paramref name="next"/> son nulos.</exception>
    /// <seealso cref="ActionExecutingContext"/>
    /// <seealso cref="ActionExecutionDelegate"/>
    public override async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next ) {
        context.CheckNull( nameof( context ) );
        next.CheckNull( nameof( next ) );
        var @lock = CreateLock( context );
        var key = GetKey( context );
        var isSuccess = false;
        try {
            isSuccess = await @lock.LockAsync( key, GetExpiration() );
            if ( isSuccess == false ) {
                context.Result = GetResult( context,StateCode.Fail, GetFailMessage( context ) );
                return;
            }
            OnActionExecuting( context );
            if ( context.Result != null )
                return;
            var executedContext = await next();
            OnActionExecuted( executedContext );
        }
        finally {
            if ( isSuccess ) {
                await @lock.UnLockAsync();
            }
        }
    }

    /// <summary>
    /// Crea una instancia de un objeto que implementa la interfaz <see cref="ILock"/>.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud HTTP actual.</param>
    /// <returns>
    /// Una instancia de <see cref="ILock"/>. Si no se puede obtener una instancia, se devuelve <see cref="NullLock.Instance"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    /// <seealso cref="ILock"/>
    /// <seealso cref="NullLock"/>
    protected virtual ILock CreateLock( ActionExecutingContext context ) {
        return context.HttpContext.RequestServices.GetService<ILock>() ?? NullLock.Instance;
    }

    /// <summary>
    /// Obtiene una clave única basada en el contexto de ejecución de la acción.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud actual.</param>
    /// <returns>
    /// Una cadena que representa la clave única generada. 
    /// Si la propiedad <see cref="Key"/> está vacía, la clave se construye a partir del ID del usuario y la ruta de la solicitud web.
    /// De lo contrario, se utiliza el valor de <see cref="Key"/> junto con el ID del usuario.
    protected virtual string GetKey( ActionExecutingContext context ) {
        var userId = string.Empty;
        if ( Type == LockType.User )
            userId = $"{GetUserId( context )}_";
        return string.IsNullOrWhiteSpace( Key ) ? $"{userId}{Web.Request.Path}" : $"{userId}{Key}";
    }

    /// <summary>
    /// Obtiene el identificador del usuario a partir del contexto de ejecución de la acción.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud actual.</param>
    /// <returns>El identificador del usuario si está disponible; de lo contrario, null.</returns>
    protected string GetUserId(ActionExecutingContext context) 
    { 
        var session = context.HttpContext.RequestServices.GetService<ISession>(); 
        return session?.UserId; 
    }

    /// <summary>
    /// Obtiene el tiempo de expiración basado en el intervalo definido.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="TimeSpan"/> que representa el tiempo de expiración, 
    /// o <c>null</c> si el intervalo es cero.
    /// </returns>
    private TimeSpan? GetExpiration() {
        if ( Interval == 0 )
            return null;
        return TimeSpan.FromSeconds( Interval );
    }

    /// <summary>
    /// Obtiene un resultado basado en el contexto de ejecución de la acción.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud HTTP actual.</param>
    /// <param name="code">El código que se incluirá en el resultado.</param>
    /// <param name="message">El mensaje que se incluirá en el resultado.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la acción.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener una instancia de <see cref="IResultFactory"/> desde los servicios del contexto HTTP.
    /// Si no se encuentra, se crea un nuevo resultado utilizando el código y el mensaje proporcionados.
    /// En caso contrario, se utiliza la fábrica de resultados para crear el resultado.
    /// </remarks>
    private IActionResult GetResult( ActionExecutingContext context,string code, string message ) {
        var options = GetJsonSerializerOptions( context );
        var resultFactory = context.HttpContext.RequestServices.GetService<IResultFactory>();
        if ( resultFactory == null )
            return new Result( code, message,options: options );
        return resultFactory.CreateResult( code, message, null, null, options );
    }

    /// <summary>
    /// Obtiene las opciones de serialización JSON para la acción en ejecución.
    /// </summary>
    /// <param name="context">El contexto de la acción que se está ejecutando.</param>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> que contiene las configuraciones de serialización JSON.
    /// </returns>
    /// <remarks>
    /// Si se proporciona un <see cref="IJsonSerializerOptionsFactory"/> en el contenedor de servicios, se utilizará para crear las opciones.
    /// De lo contrario, se devolverán opciones predeterminadas que incluyen políticas de nomenclatura y convertidores específicos.
    /// </remarks>
    private JsonSerializerOptions GetJsonSerializerOptions( ActionExecutingContext context ) {
        var factory = context.HttpContext.RequestServices.GetService<IJsonSerializerOptionsFactory>();
        if( factory != null )
            return factory.CreateOptions();
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }

    /// <summary>
    /// Obtiene un mensaje de error que indica un fallo en la ejecución de la acción.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud actual.</param>
    /// <returns>
    /// Un mensaje de error localizado que describe el motivo del fallo.
    /// </returns>
    /// <remarks>
    /// Este método verifica el tipo de bloqueo y devuelve un mensaje específico según el tipo.
    /// Si el tipo de bloqueo es 'User', se devuelve un mensaje relacionado con solicitudes duplicadas de usuario.
    /// De lo contrario, se devuelve un mensaje global para solicitudes duplicadas.
    /// </remarks>
    protected virtual string GetFailMessage(ActionExecutingContext context) {
        if (Type == LockType.User)
            return GetLocalizedMessage(context, R.UserDuplicateRequest);
        return GetLocalizedMessage(context, R.GlobalDuplicateRequest);
    }

    /// <summary>
    /// Obtiene un mensaje localizado a partir de un contexto de ejecución de acción y un mensaje dado.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud actual.</param>
    /// <param name="message">El mensaje que se desea localizar.</param>
    /// <returns>El mensaje localizado si se encuentra, de lo contrario, el mensaje original.</returns>
    /// <remarks>
    /// Este método intenta obtener un servicio de localización de cadenas del contexto HTTP.
    /// Si el servicio no está disponible, se devuelve el mensaje original sin cambios.
    /// </remarks>
    protected virtual string GetLocalizedMessage(ActionExecutingContext context, string message) 
    {
        var stringLocalizer = context.HttpContext.RequestServices.GetService<IStringLocalizer>();
        if (stringLocalizer == null)
            return message;
        return stringLocalizer[message];
    }
}
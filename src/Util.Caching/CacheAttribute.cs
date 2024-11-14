using Util.Aop;

namespace Util.Caching;

/// <summary>
/// Atributo que permite la implementación de un sistema de caché en métodos.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para interceptar llamadas a métodos y almacenar en caché los resultados
/// para mejorar el rendimiento y reducir la carga en recursos. Se puede aplicar a métodos específicos
/// donde se desea que los resultados sean almacenados y reutilizados.
/// </remarks>
public class CacheAttribute : InterceptorBase {
    /// <summary>
    /// Obtiene o establece el prefijo asociado.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el prefijo.
    /// </value>
    public string Prefix { get; set; }
    /// <summary>
    /// Representa el tiempo de expiración en segundos.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es 36000 segundos, que equivale a 10 horas.
    /// </remarks>
    /// <value>
    /// Un entero que indica el tiempo de expiración en segundos.
    /// </value>
    public int Expiration { get; set; } = 36000;

    /// <summary>
    /// Invoca el aspecto y maneja la lógica de caché para el contexto dado.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre la invocación actual.</param>
    /// <param name="next">El delegado que representa el siguiente aspecto o la invocación del método original.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método verifica si hay un valor en caché para la clave generada a partir del contexto.
    /// Si se encuentra un valor en caché, se establece como el valor de retorno del contexto.
    /// Si no se encuentra, se invoca el siguiente aspecto o método y se almacena el resultado en caché.
    /// </remarks>
    public override async Task Invoke( AspectContext context, AspectDelegate next ) {
        var cache = GetCache( context );
        var returnType = GetReturnType( context );
        var key = CreateCacheKey( context );
        var value = await GetCacheValue( cache, returnType, key );
        if( value != null ) {
            SetReturnValue( context, returnType, value );
            return;
        }
        await next( context );
        await SetCache( context, cache, key );
    }

    /// <summary>
    /// Obtiene una instancia de <see cref="ICache"/> desde el proveedor de servicios.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre el servicio.</param>
    /// <returns>
    /// Una instancia de <see cref="ICache"/> obtenida del proveedor de servicios.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual ICache GetCache(AspectContext context) 
    { 
        return context.ServiceProvider.GetService<ICache>(); 
    }

    /// <summary>
    /// Obtiene el tipo de retorno del método de servicio basado en el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre el método de servicio.</param>
    /// <returns>
    /// El tipo de retorno del método de servicio. Si el método es asíncrono, se devuelve el primer argumento genérico; 
    /// de lo contrario, se devuelve el tipo de retorno directamente.
    /// </returns>
    private Type GetReturnType( AspectContext context ) {
        return context.IsAsync() ? context.ServiceMethod.ReturnType.GetGenericArguments().First() : context.ServiceMethod.ReturnType;
    }

    /// <summary>
    /// Crea una clave de caché basada en el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto que contiene información sobre el servicio y los parámetros.</param>
    /// <returns>Una cadena que representa la clave de caché generada.</returns>
    /// <remarks>
    /// Este método utiliza un generador de claves de caché para crear una clave única
    /// que puede ser utilizada para almacenar y recuperar datos en caché. La clave se
    /// genera utilizando el método del servicio, los parámetros y un prefijo específico.
    /// </remarks>
    private string CreateCacheKey( AspectContext context ) {
        var keyGenerator = context.ServiceProvider.GetService<ICacheKeyGenerator>();
        return keyGenerator.CreateCacheKey( context.ServiceMethod, context.Parameters, GetPrefix( context ) );
    }

    /// <summary>
    /// Obtiene el prefijo formateado utilizando los parámetros del contexto especificado.
    /// </summary>
    /// <param name="context">El contexto de aspecto que contiene los parámetros a utilizar.</param>
    /// <returns>
    /// Un string que representa el prefijo formateado. Si ocurre un error durante el formateo, se devuelve el prefijo sin modificar.
    /// </returns>
    /// <remarks>
    /// Este método intenta formatear el prefijo utilizando los parámetros del contexto. 
    /// Si se produce una excepción, se captura y se devuelve el prefijo original.
    /// </remarks>
    private string GetPrefix( AspectContext context ) {
        try {
            return string.Format( Prefix, context.Parameters.ToArray() );
        }
        catch {
            return Prefix;
        }
    }

    /// <summary>
    /// Obtiene un valor del caché de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="cache">La instancia de ICache desde la cual se recuperará el valor.</param>
    /// <param name="returnType">El tipo del valor que se espera recuperar del caché.</param>
    /// <param name="key">La clave asociada al valor en el caché.</param>
    /// <returns>
    /// Un objeto que representa el valor recuperado del caché. 
    /// El tipo del objeto será el especificado por el parámetro <paramref name="returnType"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para acceder a datos almacenados en caché de manera eficiente, 
    /// evitando la necesidad de realizar operaciones costosas si el valor ya está disponible.
    /// </remarks>
    private async Task<object> GetCacheValue( ICache cache, Type returnType, string key ) {
        return await cache.GetAsync( key, returnType );
    }

    /// <summary>
    /// Establece el valor de retorno en el contexto dado, dependiendo de si la operación es asíncrona o no.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre la ejecución actual.</param>
    /// <param name="returnType">El tipo que se espera como valor de retorno.</param>
    /// <param name="value">El valor que se desea establecer como retorno.</param>
    /// <remarks>
    /// Si el contexto es asíncrono, se utiliza el método estático <c>FromResult</c> de la clase <c>Task</c> 
    /// para crear una tarea completada con el valor proporcionado. De lo contrario, se establece el valor directamente.
    /// </remarks>
    private void SetReturnValue( AspectContext context, Type returnType, object value ) {
        if( context.IsAsync() ) {
            context.ReturnValue = typeof( Task ).GetMethods()
                .First( p => p.Name == "FromResult" && p.ContainsGenericParameters )
                .MakeGenericMethod( returnType ).Invoke( null, new[] { value } );
            return;
        }
        context.ReturnValue = value;
    }

    /// <summary>
    /// Establece un valor en la caché de manera asíncrona utilizando el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre la ejecución actual.</param>
    /// <param name="cache">La instancia de la caché donde se almacenará el valor.</param>
    /// <param name="key">La clave bajo la cual se almacenará el valor en la caché.</param>
    /// <returns>Una tarea que representa la operación asíncrona de establecer el valor en la caché.</returns>
    /// <remarks>
    /// Este método verifica si el contexto es asíncrono y, en caso afirmativo, desenvuelve el valor de retorno de forma asíncrona.
    /// De lo contrario, utiliza el valor de retorno sin desenvuelto.
    /// </remarks>
    private async Task SetCache( AspectContext context, ICache cache, string key ) {
        var options = new CacheOptions { Expiration = TimeSpan.FromSeconds( Expiration ) };
        var returnValue = context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;
        await cache.SetAsync( key, returnValue, options );
    }
}
namespace Util.Microservices.Polly;

/// <summary>
/// Representa una política de reintentos utilizando la biblioteca Polly.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IRetryPolicy"/> y proporciona mecanismos para
/// manejar reintentos en operaciones que pueden fallar, permitiendo configurar el número de
/// intentos y las condiciones bajo las cuales se deben realizar.
/// </remarks>
public class PollyRetryPolicy : IRetryPolicy
{

    #region campo

    private readonly PolicyBuilder _policyBuilder;
    private readonly int? _count;
    private bool _isForever;
    private bool _isWait;
    private Func<int, TimeSpan> _sleepDurationProvider;
    private Action<Exception, int> _onRetry;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PollyRetryPolicy"/>.
    /// </summary>
    /// <param name="policyBuilder">El objeto <see cref="PolicyBuilder"/> que se utilizará para construir la política de reintentos.</param>
    /// <param name="count">El número de reintentos a realizar. Si es null, se considerará que no hay un límite específico.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="policyBuilder"/> es null.</exception>
    public PollyRetryPolicy(PolicyBuilder policyBuilder, int? count)
    {
        _policyBuilder = policyBuilder ?? throw new ArgumentNullException(nameof(policyBuilder));
        _count = count;
        _isForever = false;
        _isWait = false;
    }

    #endregion

    #region Forever

    /// <inheritdoc />
    /// <summary>
    /// Configura la política de reintentos para que se ejecute indefinidamente.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="IRetryPolicy"/> con la política de reintentos configurada para ejecutarse para siempre.
    /// </returns>
    /// <remarks>
    /// Esta función es útil cuando se desea que una operación continúe intentando ejecutarse sin límite de intentos,
    /// lo que puede ser útil en situaciones donde se espera que la operación eventualmente tenga éxito.
    /// </remarks>
    public IRetryPolicy Forever()
    {
        _isForever = true;
        return this;
    }

    #endregion

    #region Wait

    /// <inheritdoc />
    /// <summary>
    /// Indica que el objeto debe esperar antes de realizar la siguiente acción.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="IRetryPolicy"/> para permitir la encadenación de métodos.
    /// </returns>
    /// <remarks>
    /// Este método establece el estado de espera en verdadero, lo que puede afectar el comportamiento de las operaciones subsiguientes.
    /// </remarks>
    public IRetryPolicy Wait()
    {
        _isWait = true;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un proveedor de duración de espera para la política de reintento.
    /// </summary>
    /// <param name="sleepDurationProvider">Función que determina la duración de espera en función del número de reintentos.</param>
    /// <returns>La instancia actual de la política de reintento.</returns>
    /// <remarks>
    /// Este método permite configurar un comportamiento personalizado para la duración de espera entre reintentos,
    /// lo que puede ser útil para implementar estrategias de retroceso exponencial o cualquier otra lógica de espera.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    public IRetryPolicy Wait(Func<int, TimeSpan> sleepDurationProvider)
    {
        _isWait = true;
        _sleepDurationProvider = sleepDurationProvider;
        return this;
    }

    #endregion

    #region OnRetry

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cada vez que se produzca un intento de reintento.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe como parámetros una excepción y el número de intento.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IRetryPolicy"/>.</returns>
    /// <remarks>
    /// Esta función permite personalizar el comportamiento en caso de que se produzca un error y se decida reintentar la operación.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    public IRetryPolicy OnRetry(Action<Exception, int> action)
    {
        _onRetry = action;
        return this;
    }

    #endregion

    #region Execute

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción dentro de una política de reintento.
    /// </summary>
    /// <param name="action">La acción que se desea ejecutar.</param>
    /// <remarks>
    /// Este método crea una política de reintento y utiliza su método Execute
    /// para ejecutar la acción proporcionada. Si la acción falla, se aplicarán
    /// los reintentos definidos en la política.
    /// </remarks>
    /// <seealso cref="CreateRetryPolicy"/>
    public void Execute(Action action)
    {
        CreateRetryPolicy().Execute(action);
    }

    /// <summary>
    /// Crea una política de reintento basada en la configuración actual.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy"/> que define la estrategia de reintento.
    /// </returns>
    /// <remarks>
    /// Este método determina qué tipo de política de reintento crear en función de la
    /// propiedad <c>_isWait</c>. Si <c>_isWait</c> es falso, se crea una política sin espera.
    /// De lo contrario, se crea una política que incluye un período de espera entre los reintentos.
    /// </remarks>
    private RetryPolicy CreateRetryPolicy()
    {
        if (_isWait == false)
            return CreateRetryPolicyByNoWait();
        return CreateRetryPolicyByWait();
    }

    /// <summary>
    /// Crea una política de reintento que no espera entre los intentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se creará una política de reintento que se ejecutará indefinidamente.
    /// De lo contrario, se utilizará un número específico de intentos de reintento, determinado por el método <c>GetRetryCount()</c>.
    /// </remarks>
    private RetryPolicy CreateRetryPolicyByNoWait()
    {
        if (_isForever)
            return _policyBuilder.RetryForever(OnRetry);
        return _policyBuilder.Retry(GetRetryCount(), OnRetry);
    }

    /// <summary>
    /// Maneja el evento de reintento, invocando una acción definida si está disponible.
    /// </summary>
    /// <param name="exception">La excepción que se produjo durante la operación que se está reintentando.</param>
    /// <param name="times">El número de veces que se ha intentado la operación.</param>
    private void OnRetry(Exception exception, int times)
    {
        if (_onRetry == null)
            return;
        _onRetry(exception, times);
    }

    /// <summary>
    /// Obtiene el número de reintentos permitidos.
    /// </summary>
    /// <returns>
    /// Un entero que representa el número de reintentos. 
    /// Si el valor de <c>_count</c> es mayor que 0, se devuelve su valor seguro; 
    /// de lo contrario, se devuelve 1.
    /// </returns>
    private int GetRetryCount()
    {
        return _count > 0 ? _count.SafeValue() : 1;
    }

    /// <summary>
    /// Crea una política de reintento que espera entre cada intento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se utilizará una política de reintento que 
    /// continuará indefinidamente hasta que se tenga éxito. De lo contrario, se aplicará un 
    /// número específico de reintentos definido por el método <c>GetRetryCount()</c>.
    /// </remarks>
    /// <seealso cref="GetSleepDurationProvider"/>
    /// <seealso cref="OnRetry"/>
    private RetryPolicy CreateRetryPolicyByWait()
    {
        if (_isForever)
            return _policyBuilder.WaitAndRetryForever(GetSleepDurationProvider(), (e, i, t) => OnRetry(e, i));
        return _policyBuilder.WaitAndRetry(GetRetryCount(), GetSleepDurationProvider(), (e, t, i, c) => OnRetry(e, i));
    }

    /// <summary>
    /// Obtiene un proveedor de duración de sueño basado en el número de intentos de reintento.
    /// </summary>
    /// <returns>
    /// Una función que toma un entero que representa el número de intentos de reintento 
    /// y devuelve un <see cref="TimeSpan"/> que indica la duración del sueño.
    /// </returns>
    /// <remarks>
    /// Si el proveedor de duración de sueño ya ha sido establecido, se devuelve el proveedor existente.
    /// De lo contrario, se crea uno nuevo que calcula la duración del sueño como 2 elevado al número de intentos, 
    /// expresado en segundos.
    /// </remarks>
    private Func<int, TimeSpan> GetSleepDurationProvider()
    {
        if (_sleepDurationProvider == null)
            return retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
        return _sleepDurationProvider;
    }

    #endregion

    #region ExecuteAsync

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción de forma asíncrona con una política de reintento.
    /// </summary>
    /// <param name="action">La acción asíncrona que se desea ejecutar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método crea una política de reintento y ejecuta la acción proporcionada.
    /// Si la acción falla, se reintentará según la configuración de la política.
    /// </remarks>
    /// <seealso cref="CreateAsyncRetryPolicy"/>
    public Task ExecuteAsync(Func<Task> action)
    {
        return CreateAsyncRetryPolicy().ExecuteAsync(action);
    }

    /// <summary>
    /// Crea una política de reintento asíncrona basada en la configuración de espera.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isWait</c> es falsa, se crea una política de reintento sin espera.
    /// De lo contrario, se crea una política de reintento que incluye un período de espera.
    /// </remarks>
    private AsyncRetryPolicy CreateAsyncRetryPolicy()
    {
        if (_isWait == false)
            return CreateAsyncRetryPolicyByNoWait();
        return CreateAsyncRetryPolicyByWait();
    }

    /// <summary>
    /// Crea una política de reintento asíncrona sin espera entre reintentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se utilizará una política de reintento infinito.
    /// De lo contrario, se utilizará una política de reintento con un número específico de intentos,
    /// determinado por el método <c>GetRetryCount()</c>.
    /// </remarks>
    private AsyncRetryPolicy CreateAsyncRetryPolicyByNoWait()
    {
        if (_isForever)
            return _policyBuilder.RetryForeverAsync(OnRetry);
        return _policyBuilder.RetryAsync(GetRetryCount(), OnRetry);
    }

    /// <summary>
    /// Crea una política de reintento asíncrona que espera entre reintentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se utilizará una política que reintentará indefinidamente.
    /// De lo contrario, se aplicará un número específico de reintentos.
    /// </remarks>
    /// <seealso cref="AsyncRetryPolicy"/>
    private AsyncRetryPolicy CreateAsyncRetryPolicyByWait()
    {
        if (_isForever)
            return _policyBuilder.WaitAndRetryForeverAsync(GetSleepDurationProvider(), (e, i, t) => OnRetry(e, i));
        return _policyBuilder.WaitAndRetryAsync(GetRetryCount(), GetSleepDurationProvider(), (e, t, i, c) => OnRetry(e, i));
    }

    #endregion
}
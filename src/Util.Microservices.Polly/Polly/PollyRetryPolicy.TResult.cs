namespace Util.Microservices.Polly;

/// <summary>
/// Representa una política de reintento que utiliza la biblioteca Polly para manejar fallos en operaciones asíncronas.
/// </summary>
/// <typeparam name="TResult">El tipo de resultado que se espera de la operación que se está reintentando.</typeparam>
public class PollyRetryPolicy<TResult> : IRetryPolicy<TResult>
{

    #region Campo

    private readonly PolicyBuilder<TResult> _policyBuilder;
    private readonly int? _count;
    private bool _isForever;
    private bool _isWait;
    private Func<int, TimeSpan> _sleepDurationProvider;
    private Action<DelegateResult<TResult>, int> _onRetry;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PollyRetryPolicy"/>.
    /// </summary>
    /// <param name="policyBuilder">El objeto <see cref="PolicyBuilder{TResult}"/> que se utilizará para construir la política de reintento.</param>
    /// <param name="count">El número de reintentos a realizar. Si es null, se considerará un comportamiento predeterminado.</param>
    /// <remarks>
    /// Este constructor establece los valores iniciales para la política de reintento, incluyendo la verificación de que el <paramref name="policyBuilder"/> no sea nulo.
    /// </remarks>
    public PollyRetryPolicy(PolicyBuilder<TResult> policyBuilder, int? count)
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
    /// Configura la política de reintento para que se ejecute indefinidamente.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento configurada.
    /// </returns>
    /// <remarks>
    /// Esta configuración permite que las operaciones se reintenten sin un límite de intentos.
    /// Asegúrese de que esta opción sea adecuada para su caso de uso, ya que puede llevar a ciclos de reintento interminables.
    /// </remarks>
    public IRetryPolicy<TResult> Forever()
    {
        _isForever = true;
        return this;
    }

    #endregion

    #region Wait

    /// <inheritdoc />
    /// <summary>
    /// Indica que la política de reintento debe esperar antes de realizar el siguiente intento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento actual.
    /// </returns>
    /// <remarks>
    /// Este método establece la propiedad <c>_isWait</c> en <c>true</c>, lo que indica que se debe esperar
    /// antes de proceder con el siguiente intento en la lógica de reintento.
    /// </remarks>
    public IRetryPolicy<TResult> Wait()
    {
        _isWait = true;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un proveedor de duración de espera para las reintentos.
    /// </summary>
    /// <param name="sleepDurationProvider">Función que proporciona la duración de espera en función del número de reintentos.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que permite encadenar más configuraciones.</returns>
    /// <remarks>
    /// Este método permite definir cómo se calculará el tiempo de espera entre reintentos, 
    /// lo que puede ser útil para implementar estrategias de reintento más sofisticadas.
    /// </remarks>
    public IRetryPolicy<TResult> Wait(Func<int, TimeSpan> sleepDurationProvider)
    {
        _isWait = true;
        _sleepDurationProvider = sleepDurationProvider;
        return this;
    }

    #endregion

    #region OnRetry

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cada vez que se produzca un reintento.
    /// </summary>
    /// <param name="action">La acción que se ejecutará en cada reintento, recibiendo el resultado del intento anterior y el número de reintentos realizados.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que permite la configuración adicional de la política de reintentos.</returns>
    /// <remarks>
    /// Esta función permite personalizar el comportamiento durante los reintentos, lo que puede ser útil para registrar información o realizar acciones específicas en cada intento.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    public IRetryPolicy<TResult> OnRetry(Action<DelegateResult<TResult>, int> action)
    {
        _onRetry = action;
        return this;
    }

    #endregion

    #region Execute

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción proporcionada dentro de una política de reintento.
    /// </summary>
    /// <param name="action">La acción que se desea ejecutar, representada como un <see cref="Func{TResult}"/>.</param>
    /// <returns>El resultado de la acción ejecutada.</returns>
    /// <remarks>
    /// Este método crea una política de reintento y ejecuta la acción dada. Si la acción falla, se reintentará según la configuración de la política.
    /// </remarks>
    /// <seealso cref="CreateRetryPolicy"/>
    public TResult Execute(Func<TResult> action)
    {
        return CreateRetryPolicy().Execute(action);
    }

    /// <summary>
    /// Crea una política de reintento basada en la configuración actual.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Este método determina qué tipo de política de reintento crear en función de la 
    /// propiedad <c>_isWait</c>. Si <c>_isWait</c> es <c>false</c>, se crea una política 
    /// de reintento sin espera; de lo contrario, se crea una política de reintento 
    /// con espera.
    /// </remarks>
    private RetryPolicy<TResult> CreateRetryPolicy()
    {
        if (_isWait == false)
            return CreateRetryPolicyByNoWait();
        return CreateRetryPolicyByWait();
    }

    /// <summary>
    /// Crea una política de reintento que no espera entre intentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se crea una política que reintentará indefinidamente.
    /// De lo contrario, se crea una política que reintentará un número específico de veces definido por el método <c>GetRetryCount()</c>.
    /// </remarks>
    private RetryPolicy<TResult> CreateRetryPolicyByNoWait()
    {
        if (_isForever)
            return _policyBuilder.RetryForever(OnRetry);
        return _policyBuilder.Retry(GetRetryCount(), OnRetry);
    }

    /// <summary>
    /// Maneja el evento de reintento cuando una operación falla.
    /// </summary>
    /// <param name="result">El resultado de la operación que se está reintentando.</param>
    /// <param name="times">El número de veces que se ha intentado la operación.</param>
    private void OnRetry(DelegateResult<TResult> result, int times)
    {
        if (_onRetry == null)
            return;
        _onRetry(result, times);
    }

    /// <summary>
    /// Obtiene el número de reintentos permitidos.
    /// </summary>
    /// <returns>
    /// Un entero que representa el número de reintentos. 
    /// Si el contador de reintentos (_count) es mayor que cero, se devuelve su valor seguro; 
    /// de lo contrario, se devuelve 1.
    /// </returns>
    private int GetRetryCount()
    {
        return _count > 0 ? _count.SafeValue() : 1;
    }

    /// <summary>
    /// Crea una política de reintento que espera entre intentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="RetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se creará una política que reintentará indefinidamente.
    /// De lo contrario, se utilizará un número específico de reintentos definido por <c>GetRetryCount()</c>.
    /// </remarks>
    /// <seealso cref="GetSleepDurationProvider"/>
    /// <seealso cref="OnRetry(Exception, int)"/>
    /// <seealso cref="RetryPolicy{TResult}"/>
    private RetryPolicy<TResult> CreateRetryPolicyByWait()
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
    /// De lo contrario, se crea un nuevo proveedor que calcula la duración del sueño como 
    /// 2 elevado al número de intentos de reintento, expresado en segundos.
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
    /// Ejecuta de manera asíncrona una acción proporcionada, aplicando una política de reintento.
    /// </summary>
    /// <param name="action">La acción asíncrona que se va a ejecutar.</param>
    /// <returns>Una tarea que representa el resultado de la acción ejecutada.</returns>
    /// <remarks>
    /// Este método crea una política de reintento y la utiliza para ejecutar la acción.
    /// Si la acción falla, se reintentará según la configuración de la política.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <seealso cref="CreateAsyncRetryPolicy"/>
    public Task<TResult> ExecuteAsync(Func<Task<TResult>> action)
    {
        return CreateAsyncRetryPolicy().ExecuteAsync(action);
    }

    /// <summary>
    /// Crea una política de reintento asíncrona basada en la configuración de espera.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isWait</c> es falsa, se utiliza una política de reintento sin espera.
    /// De lo contrario, se utiliza una política de reintento con espera.
    /// </remarks>
    private AsyncRetryPolicy<TResult> CreateAsyncRetryPolicy()
    {
        if (_isWait == false)
            return CreateAsyncRetryPolicyByNoWait();
        return CreateAsyncRetryPolicyByWait();
    }

    /// <summary>
    /// Crea una política de reintento asíncrona que no espera entre reintentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se crea una política de reintento que reintentará indefinidamente.
    /// De lo contrario, se crea una política de reintento con un número específico de intentos, determinado por el método <c>GetRetryCount()</c>.
    /// </remarks>
    private AsyncRetryPolicy<TResult> CreateAsyncRetryPolicyByNoWait()
    {
        if (_isForever)
            return _policyBuilder.RetryForeverAsync(OnRetry);
        return _policyBuilder.RetryAsync(GetRetryCount(), OnRetry);
    }

    /// <summary>
    /// Crea una política de reintento asíncrona que espera entre intentos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="AsyncRetryPolicy{TResult}"/> que define la política de reintento.
    /// </returns>
    /// <remarks>
    /// Si la propiedad <c>_isForever</c> es verdadera, se utilizará una política de reintento que
    /// continuará indefinidamente. De lo contrario, se aplicará un número específico de reintentos.
    /// </remarks>
    /// <seealso cref="GetSleepDurationProvider"/>
    /// <seealso cref="OnRetry"/>
    /// <seealso cref="GetRetryCount"/>
    private AsyncRetryPolicy<TResult> CreateAsyncRetryPolicyByWait()
    {
        if (_isForever)
            return _policyBuilder.WaitAndRetryForeverAsync(GetSleepDurationProvider(), (e, i, t) => OnRetry(e, i));
        return _policyBuilder.WaitAndRetryAsync(GetRetryCount(), GetSleepDurationProvider(), (e, t, i, c) => OnRetry(e, i));
    }

    #endregion
}
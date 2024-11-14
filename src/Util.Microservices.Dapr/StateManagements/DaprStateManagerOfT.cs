namespace Util.Microservices.Dapr.StateManagements;

/// <summary>
/// Clase que representa un administrador de estado utilizando Dapr.
/// </summary>
/// <typeparam name="T">El tipo de datos que se almacenará en el estado.</typeparam>
public class DaprStateManager<T> : DaprStateManagerBase<IStateManager<T>>, IStateManager<T> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprStateManager"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr utilizado para realizar llamadas al servicio Dapr.</param>
    /// <param name="options">Las opciones de configuración para Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de registradores.</param>
    /// <param name="keyGenerator">El generador de claves utilizado para crear claves únicas.</param>
    public DaprStateManager( DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory, IKeyGenerator keyGenerator )
        : base( client, options, loggerFactory, keyGenerator ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos según la expresión proporcionada.
    /// </summary>
    /// <param name="expression">La expresión que define el criterio de ordenación.</param>
    /// <returns>Una instancia del gestor de estado con los elementos ordenados.</returns>
    /// <remarks>
    /// Este método permite aplicar un orden a los elementos gestionados por el estado.
    /// La expresión debe ser una función que tome un objeto de tipo <typeparamref name="T"/> 
    /// y devuelva un objeto que se utilizará para la comparación.
    /// </remarks>
    /// <typeparam name="T">El tipo de los elementos que se están gestionando.</typeparam>
    /// <seealso cref="IStateManager{T}"/>
    public IStateManager<T> OrderBy( Expression<Func<T, object>> expression ) {
        Sort.OrderBy( expression );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos en orden descendente según la expresión proporcionada.
    /// </summary>
    /// <param name="expression">Una expresión que especifica la propiedad por la cual se ordenarán los elementos.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que permite realizar más operaciones de consulta.</returns>
    /// <remarks>
    /// Este método modifica el estado de ordenación actual y permite encadenar otras operaciones de consulta.
    /// </remarks>
    public IStateManager<T> OrderByDescending( Expression<Func<T, object>> expression ) {
        Sort.OrderByDescending( expression );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición de igualdad para el filtro basado en la expresión proporcionada.
    /// </summary>
    /// <param name="expression">Una expresión que representa la propiedad a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad especificada en la expresión.</param>
    /// <returns>Una instancia del administrador de estado actual.</returns>
    /// <remarks>
    /// Este método permite agregar una condición de igualdad al filtro, lo que significa que solo se incluirán los elementos que 
    /// cumplan con la condición especificada. La expresión debe ser una expresión válida que apunte a una propiedad del tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <typeparam name="T">El tipo de los elementos que se están filtrando.</typeparam>
    /// <seealso cref="IStateManager{T}"/>
    public IStateManager<T> Equal( Expression<Func<T, object>> expression, object value ) {
        Filter.Equal( expression, value );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión en el gestor de estados.
    /// </summary>
    /// <param name="expression">Una expresión que representa la propiedad del objeto de tipo <typeparamref name="T"/> que se va a filtrar.</param>
    /// <param name="values">Una colección de valores que se utilizarán para el filtro de inclusión.</param>
    /// <returns>El gestor de estados actual con el filtro aplicado.</returns>
    /// <remarks>
    /// Este método permite agregar un filtro que verifica si el valor de la propiedad especificada está incluido en la colección de valores proporcionada.
    /// </remarks>
    /// <typeparam name="T">El tipo de objeto que maneja el gestor de estados.</typeparam>
    /// <seealso cref="IStateManager{T}"/>
    public IStateManager<T> In( Expression<Func<T, object>> expression, IEnumerable<object> values ) {
        Filter.In( expression, values );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión para el gestor de estado.
    /// </summary>
    /// <param name="expression">Una expresión que representa la propiedad del objeto de tipo <typeparamref name="T"/> que se va a filtrar.</param>
    /// <param name="values">Un conjunto de valores que se utilizarán para la comparación de inclusión.</param>
    /// <returns>Devuelve la instancia actual del gestor de estado.</returns>
    /// <remarks>
    /// Este método permite agregar un filtro que verifica si el valor de la propiedad especificada está incluido en el conjunto de valores proporcionados.
    /// </remarks>
    /// <typeparam name="T">El tipo de objeto que maneja el gestor de estado.</typeparam>
    /// <seealso cref="IStateManager{T}"/>
    public IStateManager<T> In( Expression<Func<T, object>> expression, params object[] values ) {
        Filter.In( expression, values );
        return this;
    }
}
using Util.Data.Queries;

namespace Util.Microservices;

/// <summary>
/// Define una interfaz base para un gestor de estado.
/// </summary>
/// <typeparam name="TStateManager">El tipo del gestor de estado que implementa esta interfaz.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> y está diseñada para ser utilizada
/// como un contrato para los gestores de estado que requieren un comportamiento transitorio.
/// </remarks>
public interface IStateManagerBase<out TStateManager> : ITransientDependency where TStateManager : IStateManagerBase<TStateManager> {
    /// <summary>
    /// Establece el nombre de la tienda.
    /// </summary>
    /// <param name="storeName">El nombre de la tienda que se va a establecer.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que representa el estado actualizado.</returns>
    /// <remarks>
    /// Este método permite configurar el nombre de la tienda en el sistema. 
    /// Asegúrese de que el nombre de la tienda no esté vacío o nulo antes de llamar a este método.
    /// </remarks>
    TStateManager StoreName( string storeName );
    /// <summary>
    /// Limpia el estado actual del administrador de estados.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <see cref="TStateManager"/> que representa el estado del administrador después de la limpieza.
    /// </returns>
    /// <remarks>
    /// Este método elimina todos los datos almacenados en el administrador de estados,
    /// permitiendo que se inicie un nuevo ciclo de gestión de estados.
    /// </remarks>
    TStateManager Clear();
    /// <summary>
    /// Inicia una nueva transacción y devuelve un objeto que gestiona el estado de la transacción.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <see cref="TStateManager"/> que representa la transacción iniciada.
    /// </returns>
    /// <remarks>
    /// Este método es útil para agrupar operaciones que deben ser ejecutadas de manera atómica.
    /// Si ocurre un error durante la ejecución de las operaciones, se puede deshacer la transacción.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager BeginTransaction();
    /// <summary>
    /// Configura las opciones de serialización JSON para el administrador de estado.
    /// </summary>
    /// <param name="options">Las opciones de serialización JSON que se van a configurar.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que representa el administrador de estado configurado.</returns>
    /// <remarks>
    /// Este método permite personalizar las opciones de serialización JSON, como el formato de fecha,
    /// la inclusión de propiedades nulas, y otras configuraciones que afectan cómo se serializan
    /// y deserializan los objetos.
    /// </remarks>
    TStateManager JsonSerializerOptions( JsonSerializerOptions options );
    /// <summary>
    /// Establece o actualiza el valor de una clave en el gestor de estado.
    /// </summary>
    /// <param name="key">La clave que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se asociará con la clave especificada.</param>
    /// <returns>Una instancia de <see cref="TStateManager"/> que representa el estado actualizado.</returns>
    /// <remarks>
    /// Este método permite gestionar metadatos asociados a un estado, facilitando la 
    /// recuperación y modificación de información relevante en el contexto de la 
    /// aplicación.
    /// </remarks>
    TStateManager Metadata( string key, string value );
    /// <summary>
    /// Obtiene o establece la metadata asociada a un estado.
    /// </summary>
    /// <param name="metadata">Un diccionario que contiene pares clave-valor representando la metadata.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que contiene la metadata actualizada.</returns>
    /// <remarks>
    /// Este método permite gestionar la metadata de un estado, facilitando la adición, modificación o eliminación de información asociada.
    /// </remarks>
    TStateManager Metadata( IDictionary<string, string> metadata );
    /// <summary>
    /// Elimina la metadata asociada a la clave especificada.
    /// </summary>
    /// <param name="key">La clave de la metadata que se desea eliminar.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que representa el estado actualizado después de la eliminación.</returns>
    /// <remarks>
    /// Este método busca la metadata en la colección interna y, si se encuentra, la elimina.
    /// Si la clave no existe, no se realiza ninguna acción.
    /// </remarks>
    TStateManager RemoveMetadata(string key);
    /// <summary>
    /// Establece el tipo de contenido para el gestor de estados.
    /// </summary>
    /// <param name="type">El tipo de contenido que se desea establecer.</param>
    /// <returns>Una instancia del gestor de estados con el tipo de contenido actualizado.</returns>
    /// <remarks>
    /// Este método permite especificar el tipo de contenido que se utilizará en el gestor de estados,
    /// lo que puede ser útil para definir cómo se manejarán los datos en función del tipo de contenido.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager ContentType( string type );
    /// <summary>
    /// Establece el tamaño de la página para la gestión del estado.
    /// </summary>
    /// <param name="pageSize">El número de elementos por página.</param>
    /// <returns>Una instancia de <see cref="TStateManager"/> con el tamaño de página establecido.</returns>
    /// <remarks>
    /// Este método permite limitar la cantidad de elementos que se procesan en una sola operación,
    /// facilitando la paginación y la gestión eficiente de grandes conjuntos de datos.
    /// </remarks>
    TStateManager Limit( int pageSize );
    /// <summary>
    /// Obtiene el estado del gestor de estado correspondiente al token proporcionado.
    /// </summary>
    /// <param name="token">El token entero que identifica el estado que se desea obtener.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que representa el estado asociado al token.</returns>
    /// <remarks>
    /// Este método permite acceder a la información del estado de un gestor de estado específico
    /// utilizando un identificador único. Es importante asegurarse de que el token proporcionado
    /// sea válido y esté registrado en el sistema.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager Token( int token );
    /// <summary>
    /// Establece el criterio de ordenamiento para la gestión del estado.
    /// </summary>
    /// <param name="orderBy">Una cadena que representa el criterio de ordenamiento.</param>
    /// <returns>Una instancia de <see cref="TStateManager"/> con el criterio de ordenamiento aplicado.</returns>
    /// <remarks>
    /// Este método permite especificar cómo se deben ordenar los elementos gestionados por el estado.
    /// Asegúrese de que el valor de <paramref name="orderBy"/> sea válido para evitar excepciones.
    /// </remarks>
    TStateManager OrderBy( string orderBy );
    /// <summary>
    /// Crea una instancia de <see cref="TStateManager"/> que representa una condición de igualdad 
    /// para una propiedad específica con un valor dado.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>Una instancia de <see cref="TStateManager"/> que representa la condición de igualdad.</returns>
    /// <remarks>
    /// Este método se utiliza para construir consultas que filtren elementos basados en la igualdad 
    /// de una propiedad específica. Es útil en contextos donde se necesita aplicar filtros en 
    /// colecciones o bases de datos.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager Equal( string property, object value );
    /// <summary>
    /// Compara un valor de propiedad con un valor dado y, si se cumple una condición, 
    /// realiza una acción específica en el estado del gestor.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se va a comparar la propiedad.</param>
    /// <param name="condition">Una condición que determina si se debe realizar la comparación.</param>
    /// <returns>Devuelve una instancia del gestor de estado, posiblemente modificada.</returns>
    /// <remarks>
    /// Este método es útil para establecer condiciones en las que se desea 
    /// modificar el estado basado en la comparación de propiedades. 
    /// Si la condición es verdadera, se realiza la comparación; de lo contrario, 
    /// no se efectúa ninguna acción.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager EqualIf( string property, object value, bool condition );
    /// <summary>
    /// Establece un filtro en el gestor de estado para una propiedad específica.
    /// </summary>
    /// <param name="property">El nombre de la propiedad sobre la cual se aplicará el filtro.</param>
    /// <param name="values">Una colección de valores que se utilizarán para filtrar los resultados.</param>
    /// <returns>El gestor de estado actualizado con el filtro aplicado.</returns>
    /// <remarks>
    /// Este método permite realizar consultas más específicas al gestionar el estado de los objetos,
    /// facilitando la recuperación de datos que cumplen con ciertos criterios.
    /// </remarks>
    /// <seealso cref="TStateManager"/>
    TStateManager In( string property, IEnumerable<object> values );
    /// <summary>
    /// Establece el estado de un objeto en función de la propiedad y los valores proporcionados.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a establecer.</param>
    /// <param name="values">Un conjunto de valores que se asignarán a la propiedad especificada.</param>
    /// <returns>Un objeto de tipo <see cref="TStateManager"/> que representa el estado actualizado.</returns>
    /// <remarks>
    /// Este método permite modificar el estado de un objeto de manera flexible, permitiendo múltiples valores
    /// para la propiedad especificada. Es útil en situaciones donde se requiere un manejo dinámico de propiedades.
    /// </remarks>
    /// <example>
    /// <code>
    /// TStateManager estado = new TStateManager();
    /// estado.In("Nombre", "Valor1", "Valor2");
    /// </code>
    /// </example>
    TStateManager In( string property, params object[] values );
    /// <summary>
    /// Combina múltiples condiciones de estado utilizando una operación lógica "Y".
    /// </summary>
    /// <param name="conditions">Un arreglo de condiciones de estado que se evaluarán.</param>
    /// <returns>Un objeto <see cref="TStateManager"/> que representa el resultado de la combinación de las condiciones.</returns>
    /// <remarks>
    /// Este método permite evaluar si todas las condiciones proporcionadas son verdaderas. 
    /// Si alguna de las condiciones es falsa, el resultado será falso.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    TStateManager And( params IStateCondition[] conditions );
    /// <summary>
    /// Combina múltiples condiciones de estado utilizando una operación lógica OR.
    /// </summary>
    /// <param name="conditions">Un arreglo de condiciones de estado que se evaluarán.</param>
    /// <returns>Un objeto <see cref="TStateManager"/> que representa el resultado de la combinación de las condiciones.</returns>
    /// <remarks>
    /// Esta función permite evaluar si al menos una de las condiciones proporcionadas es verdadera.
    /// Si alguna de las condiciones es verdadera, el resultado será verdadero.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    TStateManager Or( params IStateCondition[] conditions );
    /// <summary>
    /// Agrega un valor asociado a una clave de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a agregar.</typeparam>
    /// <param name="key">La clave asociada al valor que se va a agregar.</param>
    /// <param name="value">El valor que se va a agregar.</param>
    /// <param name="cancellationToken">Un token de cancelación opcional para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite agregar un valor a una colección o almacenamiento basado en clave-valor. 
    /// Si la clave ya existe, el comportamiento puede variar según la implementación.
    /// </remarks>
    /// <seealso cref="RemoveAsync{TValue}(string, CancellationToken)"/>
    /// <seealso cref="GetAsync{TValue}(string, CancellationToken)"/>
    Task AddAsync<TValue>( string key, TValue value, CancellationToken cancellationToken = default );
    /// <summary>
    /// Actualiza un valor asociado a una clave específica en un almacenamiento.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a actualizar.</typeparam>
    /// <param name="key">La clave del valor que se desea actualizar.</param>
    /// <param name="value">El nuevo valor que se asignará a la clave.</param>
    /// <param name="etag">El valor de etag que se utiliza para controlar la concurrencia.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene <c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite actualizar un valor en el almacenamiento de manera segura, utilizando el etag para evitar conflictos de concurrencia.
    /// Si el etag proporcionado no coincide con el etag actual del valor, la operación de actualización fallará.
    /// </remarks>
    /// <seealso cref="GetAsync{TValue}(string, CancellationToken)"/>
    /// <seealso cref="DeleteAsync(string, string, CancellationToken)"/>
    Task<bool> UpdateAsync<TValue>( string key, TValue value, string etag, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar un elemento de una colección o almacenamiento basado en la clave proporcionada.
    /// Si la clave no existe, no se realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveAsync( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <typeparam name="TValue">El tipo del elemento que se va a eliminar. Debe implementar la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar un elemento de la colección o base de datos asociada.
    /// Asegúrese de que el identificador proporcionado sea válido y que el elemento exista.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    Task RemoveByIdAsync<TValue>( string id, CancellationToken cancellationToken = default ) where TValue : IDataKey;
    /// <summary>
    /// Guarda de manera asíncrona un valor de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a guardar. Debe implementar las interfaces <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="value">El valor que se va a guardar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <param name="key">Una clave opcional que se puede utilizar para identificar el valor guardado. Si es <c>null</c>, se utilizará una clave generada automáticamente.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que puede contener información sobre el resultado de la operación.</returns>
    /// <remarks>
    /// Este método es útil para almacenar datos que requieren un identificador y una etiqueta de entidad. 
    /// Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/> para permitir la cancelación de la operación si es necesario.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    /// <seealso cref="IETag"/>
    Task<string> SaveAsync<TValue>( TValue value, CancellationToken cancellationToken = default, string key = null ) where TValue : IDataKey, IETag;
    /// <summary>
    /// Guarda de manera asíncrona una colección de valores.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores a guardar. Debe implementar las interfaces <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="values">Una colección de valores que se van a guardar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <see cref="CancellationToken.None"/>.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar los valores.</returns>
    /// <remarks>
    /// Este método permite guardar múltiples valores de forma eficiente y puede ser cancelado si es necesario.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    /// <seealso cref="IETag"/>
    Task SaveAsync<TValue>( IEnumerable<TValue> values, CancellationToken cancellationToken = default ) where TValue : IDataKey, IETag;
    /// <summary>
    /// Realiza la confirmación de una tarea de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de confirmación.</returns>
    /// <remarks>
    /// Este método se utiliza para confirmar los cambios realizados en una tarea. 
    /// Si se proporciona un token de cancelación y la operación es cancelada, 
    /// se lanzará una excepción <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task CommitAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una clave asociada a un identificador específico de manera asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de datos que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador de la clave que se desea obtener.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene la clave asociada al identificador.</returns>
    /// <remarks>
    /// Este método permite recuperar claves de manera eficiente y puede ser utilizado en escenarios donde se requiere acceso a datos de forma no bloqueante.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="id"/> es null.</exception>
    /// <seealso cref="IDataKey"/>
    Task<string> GetKeyByIdAsync<TValue>( string id, CancellationToken cancellationToken = default ) where TValue : IDataKey;
    /// <summary>
    /// Obtiene el estado y el ETag asociado a una clave específica de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se recuperará.</typeparam>
    /// <param name="key">La clave asociada al estado que se desea obtener.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor que contiene el estado y el ETag.</returns>
    /// <remarks>
    /// Este método permite recuperar tanto el valor asociado a la clave como su ETag, lo que puede ser útil para la gestión de concurrencia y la verificación de cambios.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<(TValue value, string etag)> GetStateAndETagAsync<TValue>( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un valor de forma asíncrona asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el valor asociado a la clave.</returns>
    /// <remarks>
    /// Este método permite recuperar valores de manera asíncrona, lo que es útil para operaciones que pueden tardar en completarse,
    /// como el acceso a bases de datos o servicios web. Asegúrese de manejar adecuadamente el token de cancelación para evitar
    /// operaciones innecesarias si la tarea es cancelada.
    /// </remarks>
    /// <seealso cref="Task{TValue}"/>
    Task<TValue> GetAsync<TValue>( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una lista de valores de forma asíncrona a partir de una lista de claves.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se van a recuperar.</typeparam>
    /// <param name="keys">Una lista de claves que se utilizarán para obtener los valores.</param>
    /// <param name="parallelism">El número máximo de operaciones paralelas que se pueden realizar. Si es nulo, se utilizará un valor predeterminado.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene una lista de valores obtenidos.</returns>
    /// <remarks>
    /// Este método permite la recuperación de valores de manera eficiente, aprovechando la posibilidad de realizar operaciones en paralelo.
    /// El parámetro <paramref name="parallelism"/> puede ser utilizado para limitar el número de operaciones que se ejecutan simultáneamente.
    /// Si se proporciona un valor nulo, se aplicará una configuración predeterminada para el paralelismo.
    /// </remarks>
    /// <seealso cref="IList{TValue}"/>
    Task<IList<TValue>> GetAsync<TValue>( IReadOnlyList<string> keys, int? parallelism = 0, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un elemento de tipo <typeparamref name="TValue"/> de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <typeparam name="TValue">El tipo del elemento que se desea obtener. Debe implementar la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador del elemento que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el elemento de tipo <typeparamref name="TValue"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
    /// <remarks>
    /// Este método es útil para recuperar datos de una fuente de datos, como una base de datos o un servicio web.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la operación.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    Task<TValue> GetByIdAsync<TValue>( string id, CancellationToken cancellationToken = default ) where TValue : IDataKey;
    /// <summary>
    /// Asynchronously obtiene un único valor de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <typeparam name="TValue">
    /// El tipo del valor que se desea obtener, el cual debe implementar la interfaz <see cref="IDataKey"/>.
    /// </typeparam>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene el valor obtenido de tipo <typeparamref name="TValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil cuando se espera que la operación devuelva un único elemento. 
    /// Si se encuentra más de un elemento, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si se encuentra más de un elemento en la operación.
    /// </exception>
    /// <seealso cref="IDataKey"/>
    Task<TValue> SingleAsync<TValue>( CancellationToken cancellationToken = default ) where TValue : IDataKey;
    /// <summary>
    /// Obtiene una lista de todos los valores de tipo <typeparamref name="TValue"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se van a recuperar.</typeparam>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de valores de tipo <typeparamref name="TValue"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite recuperar todos los elementos de un origen de datos de forma asíncrona,
    /// lo que puede ser útil para evitar bloquear el hilo de la interfaz de usuario en aplicaciones
    /// que requieren una respuesta rápida.
    /// </remarks>
    /// <seealso cref="GetAsync{TValue}(CancellationToken)"/>
    Task<IList<TValue>> GetAllAsync<TValue>( CancellationToken cancellationToken = default );
    /// <summary>
    /// Realiza una consulta asíncrona y devuelve una lista de valores de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se devolverán en la lista.</typeparam>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene una lista de valores de tipo <typeparamref name="TValue"/>.</returns>
    /// <remarks>
    /// Este método permite realizar consultas que pueden ser canceladas si es necesario.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<IList<TValue>> QueryAsync<TValue>( CancellationToken cancellationToken = default );
    /// <summary>
    /// Realiza una consulta paginada de elementos de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los elementos que se van a paginar.</typeparam>
    /// <param name="page">La información de la página que se va a consultar.</param>
    /// <param name="cancellationToken">El token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <see cref="PageList{TValue}"/> que contiene los elementos paginados.</returns>
    /// <remarks>
    /// Este método permite obtener una lista de elementos de tipo <typeparamref name="TValue"/> 
    /// en función de los parámetros de paginación especificados en el objeto <paramref name="page"/>.
    /// </remarks>
    /// <seealso cref="PageList{TValue}"/>
    Task<PageList<TValue>> PageQueryAsync<TValue>( IPage page, CancellationToken cancellationToken = default );
}
using Util.Data.Queries;
using Util.Helpers;
using Util.Microservices.Dapr.StateManagements.Queries;

namespace Util.Microservices.Dapr.StateManagements;

/// <summary>
/// Clase base para la gestión del estado en Dapr.
/// </summary>
/// <typeparam name="TStateManager">El tipo del gestor de estado que se utilizará.</typeparam>
public partial class DaprStateManagerBase<TStateManager> {

    #region Limit

    /// <summary>
    /// Establece el tamaño de página para la gestión del estado.
    /// </summary>
    /// <param name="pageSize">El número máximo de elementos por página.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    public TStateManager Limit( int pageSize ) {
        Page.Limit = pageSize;
        return Return();
    }

    #endregion

    #region Token

    /// <summary>
    /// Establece el token en la página actual y devuelve el administrador de estado.
    /// </summary>
    /// <param name="token">El valor del token que se establecerá en la página.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TStateManager"/> que representa el administrador de estado.</returns>
    public TStateManager Token( int token ) {
        Page.Token = token.ToString();
        return Return();
    }

    #endregion

    #region OrderBy

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos según el criterio especificado.
    /// </summary>
    /// <param name="orderBy">Una cadena que representa el criterio de ordenamiento.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    /// <remarks>
    /// Este método aplica el ordenamiento a los elementos gestionados por el estado actual
    /// utilizando el criterio proporcionado en el parámetro <paramref name="orderBy"/>.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TStateManager OrderBy( string orderBy ) {
        Sort.OrderBy( orderBy );
        return Return();
    }

    #endregion

    #region Equal

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición de igualdad para un filtro basado en la propiedad y el valor especificados.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se utilizará para la comparación.</param>
    /// <param name="value">El valor que se comparará con la propiedad especificada.</param>
    /// <returns>Una instancia del administrador de estado actual.</returns>
    /// <remarks>
    /// Este método permite agregar una condición de igualdad a un filtro, lo que facilita la construcción de consultas
    /// basadas en criterios específicos. Se puede utilizar en el contexto de la manipulación de datos o la creación de
    /// consultas dinámicas.
    /// </remarks>
    /// <seealso cref="Filter"/>
    /// <seealso cref="Return"/>
    public TStateManager Equal( string property, object value ) {
        Filter.Equal( property, value );
        return Return();
    }

    #endregion

    #region EqualIf

    /// <inheritdoc />
    /// <summary>
    /// Compara un valor de propiedad con un valor dado y devuelve un estado basado en una condición.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <param name="condition">La condición que determina si se debe realizar la comparación.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TStateManager"/> que representa el resultado de la comparación.
    /// </returns>
    /// <remarks>
    /// Si la condición es verdadera, se llama al método <see cref="Equal(string, object)"/> 
    /// para realizar la comparación. Si la condición es falsa, se llama al método <see cref="Return()"/>.
    /// </remarks>
    /// <typeparam name="TStateManager">El tipo del administrador de estado que se está utilizando.</typeparam>
    public TStateManager EqualIf( string property, object value, bool condition ) {
        return condition ? Equal( property, value ) : Return();
    }

    #endregion

    #region In

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión para una propiedad específica.
    /// </summary>
    /// <param name="property">El nombre de la propiedad a la que se aplicará el filtro.</param>
    /// <param name="values">Una colección de valores que se incluirán en el filtro.</param>
    /// <returns>Una instancia del administrador de estado actual.</returns>
    /// <remarks>
    /// Este método permite filtrar los resultados basándose en una lista de valores permitidos para una propiedad específica.
    /// Se utiliza comúnmente en consultas donde se desea restringir los resultados a un conjunto específico de valores.
    /// </remarks>
    /// <seealso cref="Filter"/>
    /// <seealso cref="Return"/>
    public TStateManager In( string property, IEnumerable<object> values ) {
        Filter.In( property, values );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión para una propiedad específica.
    /// </summary>
    /// <param name="property">El nombre de la propiedad sobre la que se aplicará el filtro.</param>
    /// <param name="values">Los valores que se incluirán en el filtro.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    /// <remarks>
    /// Este método permite agregar múltiples valores a un filtro de inclusión,
    /// facilitando la construcción de consultas más complejas.
    /// </remarks>
    /// <seealso cref="Filter"/>
    /// <seealso cref="TStateManager"/>
    public TStateManager In( string property, params object[] values ) {
        Filter.In( property, values );
        return Return();
    }

    #endregion

    #region And

    /// <inheritdoc />
    /// <summary>
    /// Combina las condiciones de estado especificadas utilizando una operación lógica "Y".
    /// </summary>
    /// <param name="conditions">Un arreglo de condiciones de estado que se combinarán.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="conditions"/> es nulo, se devolverá el gestor de estado actual sin realizar ninguna operación.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    public TStateManager And( params IStateCondition[] conditions ) {
        if ( conditions == null )
            return Return();
        foreach ( var condition in conditions )
            Filter.And( condition );
        return Return();
    }

    #endregion

    #region Or

    /// <inheritdoc />
    /// <summary>
    /// Combina múltiples condiciones de estado utilizando una operación lógica "O".
    /// </summary>
    /// <param name="conditions">Un arreglo de condiciones de estado que se combinarán.</param>
    /// <returns>Una instancia del administrador de estado actual.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="conditions"/> es nulo, se devolverá el administrador de estado actual sin realizar ninguna operación.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    /// <seealso cref="Filter"/>
    public TStateManager Or( params IStateCondition[] conditions ) {
        if ( conditions == null )
            return Return();
        foreach ( var condition in conditions )
            Filter.Or( condition );
        return Return();
    }

    #endregion

    #region GetKeyByIdAsync

    /// <summary>
    /// Obtiene la clave asociada a un identificador específico de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de valor que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador del cual se desea obtener la clave.</param>
    /// <param name="cancellationToken">Token opcional para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene la clave asociada al identificador especificado,
    /// o <c>null</c> si el identificador está vacío o no se encuentra la clave.
    /// </returns>
    /// <remarks>
    /// Este método establece una condición de tipo de dato y realiza una consulta al cliente para obtener el estado
    /// asociado al identificador proporcionado. Si el identificador es vacío, se devuelve <c>null</c> inmediatamente.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    public virtual async Task<string> GetKeyByIdAsync<TValue>( string id, CancellationToken cancellationToken = default ) where TValue : IDataKey {
        if ( id.IsEmpty() )
            return null;
        SetDataTypeCondition<TValue>();
        Page.Limit = 1;
        Equal( "id", id );
        var query = CreateQuery();
        var response = await Client.QueryStateAsync<TValue>( GetStoreName(), query, Metadatas, cancellationToken );
        var result = response.Results.FirstOrDefault();
        ClearQuery();
        return result?.Key;
    }

    #endregion

    #region GetStateAndETagAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el estado y el ETag asociado a una clave específica de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se espera recuperar.</typeparam>
    /// <param name="key">La clave del estado que se desea obtener.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor que contiene una tupla con el valor recuperado y el ETag asociado.
    /// </returns>
    /// <remarks>
    /// Este método realiza una llamada al cliente para obtener el estado y el ETag, y luego limpia la consulta actual.
    /// </remarks>
    /// <seealso cref="Client"/>
    public virtual async Task<(TValue value, string etag)> GetStateAndETagAsync<TValue>( string key, CancellationToken cancellationToken = default ) {
        var result = await Client.GetStateAndETagAsync<TValue>( GetStoreName(), key, ConsistencyMode, Metadatas, cancellationToken );
        ClearQuery();
        return result;
    }

    #endregion

    #region GetAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de forma asíncrona a partir de una clave especificada.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave del valor que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{TValue}"/> que representa la operación asíncrona, que contiene el valor asociado a la clave especificada,
    /// o el valor predeterminado si no se encuentra ningún valor.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo de valor es un ETag. Si no lo es, se obtiene el estado directamente.
    /// Si el tipo es un ETag, se llama a un método diferente que también devuelve el ETag asociado.
    /// </remarks>
    /// <seealso cref="GetStateAndETagAsync{TValue}(string, CancellationToken)"/>
    /// <seealso cref="Client.GetStateAsync{TValue}(string, string, ConsistencyMode, object, CancellationToken)"/>
    public virtual async Task<TValue> GetAsync<TValue>( string key, CancellationToken cancellationToken = default ) {
        if ( IsETag<TValue>() == false ) {
            var state = await Client.GetStateAsync<TValue>( GetStoreName(), key, ConsistencyMode, Metadatas, cancellationToken );
            ClearQuery();
            return state;
        }
        var result = await GetStateAndETagAsync<TValue>( key, cancellationToken );
        if ( result.value == null )
            return default;
        if ( result.value is IETag eTag )
            eTag.ETag = result.etag;
        return result.value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene de manera asíncrona una lista de valores a partir de una lista de claves.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se obtendrán.</typeparam>
    /// <param name="keys">Una lista de claves para las cuales se desean obtener los valores.</param>
    /// <param name="parallelism">El nivel de paralelismo a utilizar en la operación. Si es nulo o cero, se utilizará el valor predeterminado.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona.</param>
    /// <returns>Una lista de valores obtenidos a partir de las claves proporcionadas.</returns>
    /// <remarks>
    /// Este método realiza una llamada a un cliente para obtener el estado en bloque de las claves especificadas.
    /// Si no se encuentran elementos, se devuelve una lista vacía.
    /// Se asegura de que los objetos que implementan la interfaz <see cref="IETag"/> tengan su propiedad ETag actualizada.
    /// </remarks>
    /// <seealso cref="IETag"/>
    public virtual async Task<IList<TValue>> GetAsync<TValue>( IReadOnlyList<string> keys, int? parallelism = 0, CancellationToken cancellationToken = default ) {
        var result = new List<TValue>();
        var items = await Client.GetBulkStateAsync( GetStoreName(), keys, parallelism, Metadatas, cancellationToken );
        if ( items == null || items.Count == 0 )
            return result;
        foreach ( var item in items ) {
            var state = Json.ToObject<TValue>( item.Value, SerializerOptions );
            if ( state is IETag eTag )
                eTag.ETag = item.ETag;
            result.Add( state );
        }
        ClearQuery();
        return result;
    }

    #endregion

    #region GetByIdAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un objeto del tipo especificado por su identificador de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo del objeto que se desea obtener. Debe implementar la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador del objeto que se desea recuperar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto del tipo <typeparamref name="TValue"/> si se encuentra; de lo contrario, <c>default</c>.
    /// </returns>
    /// <remarks>
    /// Este método primero verifica si el identificador proporcionado está vacío. Si es así, retorna el valor por defecto para el tipo <typeparamref name="TValue"/>.
    /// Si el identificador no está vacío, se genera una clave utilizando el generador de claves y se intenta obtener el objeto asociado a esa clave.
    /// Si no se encuentra el objeto, se intenta obtener la clave a partir del identificador y se realiza una segunda búsqueda.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    public virtual async Task<TValue> GetByIdAsync<TValue>( string id, CancellationToken cancellationToken = default ) where TValue : IDataKey {
        if ( id.IsEmpty() )
            return default;
        var key = KeyGenerator.CreateKey<TValue>( id );
        var result = await GetAsync<TValue>( key, cancellationToken );
        if ( result != null )
            return result;
        key = await GetKeyByIdAsync<TValue>( id, cancellationToken );
        return await GetAsync<TValue>( key, cancellationToken );
    }

    #endregion

    #region SingleAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un único valor de tipo <typeparamref name="TValue"/> de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de valor que se desea obtener, que debe implementar <see cref="IDataKey"/>.</typeparam>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor que contiene el único elemento de tipo <typeparamref name="TValue"/> obtenido.
    /// Si no se encuentra ningún elemento, se devolverá <c lang="null"/>.
    /// </returns>
    /// <remarks>
    /// Este método establece una condición de tipo de datos y limita el número de resultados a uno antes de realizar la consulta.
    /// Luego, intenta obtener el elemento por su identificador si se encuentra uno en el resultado.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    public virtual async Task<TValue> SingleAsync<TValue>( CancellationToken cancellationToken = default ) where TValue : IDataKey {
        SetDataTypeCondition<TValue>();
        Page.Limit = 1;
        var result = await QueryAsync<TValue>( cancellationToken );
        var value = result.FirstOrDefault();
        return await GetByIdAsync<TValue>( value?.Id, cancellationToken );
    }

    #endregion

    #region GetAllAsync

    /// <summary>
    /// Obtiene una lista de todos los elementos de un tipo específico de manera asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de elementos del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método establece primero la condición del tipo de datos antes de realizar la consulta.
    /// </remarks>
    /// <seealso cref="SetDataTypeCondition{TValue}"/>
    /// <seealso cref="QueryAsync{TValue}(CancellationToken)"/>
    public virtual async Task<IList<TValue>> GetAllAsync<TValue>( CancellationToken cancellationToken = default ) {
        SetDataTypeCondition<TValue>();
        return await QueryAsync<TValue>( cancellationToken );
    }

    #endregion

    #region QueryAsync

    /// <summary>
    /// Realiza una consulta asíncrona y devuelve una lista de valores de tipo especificado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se devolverán en la lista.</typeparam>
    /// <param name="cancellationToken">Token de cancelación para permitir la cancelación de la operación asíncrona.</param>
    /// <returns>
    /// Una lista de valores de tipo <typeparamref name="TValue"/> obtenidos de la consulta.
    /// </returns>
    /// <remarks>
    /// Este método crea una consulta utilizando el método <see cref="CreateQuery"/> y luego realiza la consulta
    /// utilizando el cliente configurado. Si la respuesta es nula, se devuelve una lista vacía. 
    /// Los elementos de la respuesta se añaden a la lista resultante, y si el elemento tiene un ETag, se actualiza
    /// el valor correspondiente.
    /// </remarks>
    /// <seealso cref="CreateQuery"/>
    /// <seealso cref="Client.QueryStateAsync{TValue}"/>
    /// <seealso cref="ClearQuery"/>
    public virtual async Task<IList<TValue>> QueryAsync<TValue>( CancellationToken cancellationToken = default ) {
        var result = new List<TValue>();
        var query = CreateQuery();
        var response = await Client.QueryStateAsync<TValue>( GetStoreName(), query, Metadatas, cancellationToken );
        ClearQuery();
        if ( response == null )
            return result;
        foreach ( var item in response.Results ) {
            if ( item.Data is IETag eTag )
                eTag.ETag = item.ETag;
            result.Add( item.Data );
        }
        return result;
    }

    /// <summary>
    /// Crea una consulta en formato JSON basada en el estado actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta en formato JSON.
    /// </returns>
    protected virtual string CreateQuery() {
        var query = new StateQuery {
            Filter = Filter.GetCondition(),
            Sort = Sort,
            Page = Page
        };
        return Json.ToJson(query, SerializerOptions);
    }

    #endregion

    #region PageQueryAsync

    /// <summary>
    /// Realiza una consulta paginada de datos de tipo <typeparamref name="TValue"/> de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los datos que se devolverán en la consulta.</typeparam>
    /// <param name="page">Un objeto que contiene la información de paginación, como el tamaño de la página y el orden.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es un objeto <see cref="PageList{TValue}"/> que contiene los datos paginados.
    /// </returns>
    /// <remarks>
    /// Este método establece el orden y los límites de la consulta según los parámetros proporcionados en el objeto <paramref name="page"/>.
    /// Luego, realiza la consulta asíncrona y devuelve los resultados en un objeto de tipo <see cref="PageList{TValue}"/>.
    /// </remarks>
    /// <seealso cref="PageList{TValue}"/>
    public virtual async Task<PageList<TValue>> PageQueryAsync<TValue>( IPage page, CancellationToken cancellationToken = default ) {
        Sort.OrderBy( page.Order );
        Page.Limit = page.PageSize;
        Page.Token = page.GetSkipCount() > 0 ? page.GetSkipCount().ToString() : null;
        var data = await QueryAsync<TValue>( cancellationToken );
        return new PageList<TValue>( page, data );
    }

    #endregion
}
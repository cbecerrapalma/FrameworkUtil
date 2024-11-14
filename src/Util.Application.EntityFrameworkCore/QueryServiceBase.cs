using Microsoft.EntityFrameworkCore;
using Util.Data;
using Util.Data.EntityFrameworkCore;
using Util.Data.Queries;
using Util.Data.Stores;
using Util.Domain;

namespace Util.Applications;

/// <summary>
/// Clase base abstracta para servicios de consulta que manejan entidades, DTOs y consultas específicas.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se está consultando.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos (DTO) asociado a la entidad.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa la consulta.</typeparam>
public abstract class QueryServiceBase<TEntity, TDto, TQuery> : QueryServiceBase<TEntity, TDto, TQuery, Guid>
    where TEntity : class, IKey<Guid>
    where TDto : new()
    where TQuery : IPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QueryServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="store">La tienda de consultas que maneja las entidades de tipo <typeparamref name="TEntity"/>.</param>
    protected QueryServiceBase(IServiceProvider serviceProvider, IQueryStore<TEntity, Guid> store) : base(serviceProvider, store) { }
}

/// <summary>
/// Clase base abstracta para servicios de consulta que manejan entidades de tipo <typeparamref name="TEntity"/> 
/// y sus correspondientes DTOs de tipo <typeparamref name="TDto"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se va a consultar.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa la entidad.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa la consulta.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación base para los servicios de consulta, 
/// permitiendo la manipulación y recuperación de datos de forma estructurada.
/// </remarks>
public abstract class QueryServiceBase<TEntity, TDto, TQuery, TKey> : ServiceBase, IQueryService<TDto, TQuery>
    where TEntity : class, IKey<TKey>
    where TDto : new()
    where TQuery : IPage
{

    #region Campo

    private readonly IQueryStore<TEntity, TKey> _store;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QueryServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="store">La tienda de consultas que se utilizará para acceder a los datos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="store"/> es <c>null</c>.</exception>
    protected QueryServiceBase(IServiceProvider serviceProvider, IQueryStore<TEntity, TKey> store) : base(serviceProvider)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    #endregion

    #region atributo

    /// <summary>
    /// Indica si se está realizando un seguimiento.
    /// </summary>
    /// <value>
    /// Devuelve <c>false</c> ya que este método es virtual y puede ser sobreescrito en una clase derivada.
    /// </value>
    protected virtual bool IsTracking => false;

    #endregion

    #region ToDto(Conversión)

    /// <summary>
    /// Convierte una entidad del tipo <typeparamref name="TEntity"/> a un objeto de transferencia de datos (DTO) del tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <param name="entity">La entidad que se va a convertir.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TDto"/> que representa la entidad convertida.</returns>
    /// <typeparam name="TEntity">El tipo de la entidad que se está convirtiendo.</typeparam>
    /// <typeparam name="TDto">El tipo del objeto de transferencia de datos al que se está convirtiendo la entidad.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una conversión personalizada.
    /// </remarks>
    protected virtual TDto ToDto(TEntity entity)
    {
        return entity.MapTo<TDto>();
    }

    #endregion

    #region GetAllAsync(Obtener todas las entidades.)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de todos los elementos de tipo <typeparamref name="TDto"/> de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con una lista de objetos de tipo <typeparamref name="TDto"/> como resultado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetStore"/> para acceder a la fuente de datos y recuperar todos los elementos.
    /// Luego, convierte cada entidad en un objeto de tipo <typeparamref name="TDto"/> utilizando el método <see cref="ToDto"/>.
    /// </remarks>
    /// <typeparam name="TDto">
    /// El tipo de objeto de transferencia de datos que se devolverá.
    /// </typeparam>
    public virtual async Task<List<TDto>> GetAllAsync()
    {
        var entities = await GetStore().FindAllAsync();
        return entities.Select(ToDto).ToList();
    }

    /// <summary>
    /// Obtiene la tienda de consultas para la entidad especificada.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IQueryStore{TEntity, TKey}"/> que puede ser rastreada o no, 
    /// dependiendo del estado de seguimiento actual.
    /// </returns>
    /// <remarks>
    /// Si el seguimiento está habilitado, se devuelve la tienda de consultas original. 
    /// Si el seguimiento está deshabilitado, se devuelve una versión de la tienda que no rastrea cambios.
    /// </remarks>
    private IQueryStore<TEntity, TKey> GetStore()
    {
        if (IsTracking)
            return _store;
        return _store.NoTracking();
    }

    #endregion

    #region GetByIdAsync(Obtener entidad a través de la identificación.)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un objeto de tipo <typeparamref name="TDto"/> por su identificador.
    /// </summary>
    /// <param name="id">El identificador del objeto que se desea obtener.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de la tarea contiene el objeto de tipo <typeparamref name="TDto"/> correspondiente al identificador proporcionado, o null si no se encuentra.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el almacenamiento para buscar el objeto por su identificador y lo convierte a un objeto de tipo <typeparamref name="TDto"/>.
    /// </remarks>
    /// <typeparam name="TDto">El tipo del objeto de transferencia de datos que se devolverá.</typeparam>
    /// <typeparam name="TKey">El tipo del identificador del objeto que se está buscando.</typeparam>
    /// <seealso cref="GetStore"/>
    public virtual async Task<TDto> GetByIdAsync(object id)
    {
        var key = Util.Helpers.Convert.To<TKey>(id);
        return ToDto(await GetStore().FindByIdAsync(key));
    }

    #endregion

    #region GetByIdsAsync(Obtener una lista de entidades a través de una lista de identificadores.)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de objetos DTO a partir de una cadena de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los objetos a recuperar, separados por comas.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una lista de objetos DTO correspondientes a los identificadores proporcionados.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="GetStore().FindByIdsAsync(string)"/> para recuperar las entidades
    /// y luego las convierte a DTO utilizando el método <see cref="ToDto"/>.
    /// </remarks>
    /// <seealso cref="GetStore"/>
    /// <seealso cref="ToDto"/>
    public virtual async Task<List<TDto>> GetByIdsAsync(string ids)
    {
        var entities = await GetStore().FindByIdsAsync(ids);
        return entities.Select(ToDto).ToList();
    }

    #endregion

    #region QueryAsync(Consulta)

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una consulta asíncrona y devuelve una lista de objetos de tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <param name="param">Los parámetros de consulta de tipo <typeparamref name="TQuery"/> que se utilizarán para filtrar los resultados.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de objetos de tipo <typeparamref name="TDto"/> como resultado.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="param"/> es nulo, se devolverá una lista vacía.
    /// La consulta se construye utilizando condiciones y filtros aplicados a la fuente de datos.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto que se devolverá en la lista.</typeparam>
    /// <typeparam name="TQuery">El tipo de objeto que representa los parámetros de consulta.</typeparam>
    /// <seealso cref="GetStore"/>
    /// <seealso cref="AddConditions"/>
    /// <seealso cref="Filter"/>
    /// <seealso cref="ToDto"/>
    public virtual async Task<List<TDto>> QueryAsync(TQuery param)
    {
        if (param == null)
            return new List<TDto>();
        var queryable = GetStore().Find();
        queryable = AddConditions(queryable, param);
        queryable = Filter(queryable, param);
        var result = await queryable.OrderBy(param).ToListAsync();
        return result.Select(ToDto).ToList();
    }

    /// <summary>
    /// Agrega condiciones a una consulta IQueryable basada en los parámetros proporcionados.
    /// </summary>
    /// <param name="queryable">La consulta IQueryable a la que se le agregarán las condiciones.</param>
    /// <param name="param">Los parámetros que se utilizarán para obtener las condiciones.</param>
    /// <returns>
    /// Una nueva consulta IQueryable que incluye las condiciones aplicadas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método GetConditions para obtener una colección de condiciones 
    /// basadas en los parámetros proporcionados. Si no se encuentran condiciones, se devuelve 
    /// la consulta original sin modificaciones.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <typeparam name="TQuery">El tipo de los parámetros utilizados para obtener las condiciones.</typeparam>
    private IQueryable<TEntity> AddConditions(IQueryable<TEntity> queryable, TQuery param)
    {
        var conditions = GetConditions(param);
        if (conditions == null)
            return queryable;
        foreach (var condition in conditions)
            queryable = queryable.Where(condition);
        return queryable;
    }

    /// <summary>
    /// Obtiene las condiciones basadas en el parámetro proporcionado.
    /// </summary>
    /// <param name="param">El parámetro de consulta utilizado para obtener las condiciones.</param>
    /// <returns>
    /// Una colección de condiciones que implementan la interfaz <see cref="ICondition{TEntity}"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo del parámetro de consulta.</typeparam>
    /// <typeparam name="TEntity">El tipo de entidad para la cual se están generando las condiciones.</typeparam>
    protected virtual IEnumerable<ICondition<TEntity>> GetConditions(TQuery param)
    {
        return null;
    }

    /// <summary>
    /// Filtra una consulta de entidades según los parámetros especificados.
    /// </summary>
    /// <param name="queryable">La consulta de entidades que se va a filtrar.</param>
    /// <param name="param">Los parámetros utilizados para aplicar el filtro.</param>
    /// <returns>
    /// Una consulta filtrada de entidades que coincide con los parámetros dados.
    /// </returns>
    protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> queryable, TQuery param)
    {
        return queryable;
    }

    #endregion

    #region PageQueryAsync(Consulta paginada)

    /// <inheritdoc />
    /// <summary>
    /// Realiza una consulta paginada de elementos de tipo <typeparamref name="TDto"/> 
    /// basada en los parámetros proporcionados.
    /// </summary>
    /// <param name="param">Los parámetros de consulta que se utilizarán para filtrar y paginar los resultados.</param>
    /// <returns>
    /// Un objeto <see cref="PageList{TDto}"/> que contiene los elementos paginados resultantes de la consulta.
    /// Si <paramref name="param"/> es <c>null</c>, se devolverá una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método es asíncrono y utiliza el patrón de diseño de repositorio para acceder a los datos. 
    /// Asegúrese de que los métodos auxiliares como <see cref="GetStore"/>, <see cref="AddConditions"/>, 
    /// <see cref="Filter"/> y <see cref="ToPageListAsync"/> estén correctamente implementados 
    /// para garantizar el funcionamiento adecuado de la consulta.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto de datos de transferencia que se devolverá en la lista paginada.</typeparam>
    public virtual async Task<PageList<TDto>> PageQueryAsync(TQuery param)
    {
        if (param == null)
            return new PageList<TDto>();
        var queryable = GetStore().Find();
        queryable = AddConditions(queryable, param);
        queryable = Filter(queryable, param);
        var result = await queryable.ToPageListAsync(param);
        return result.Convert(ToDto);
    }

    #endregion
}